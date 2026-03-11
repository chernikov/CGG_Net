using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CGG.Application.DTOs.Credits;
using CGG.Application.Interfaces;
using CGG.Application.Settings;
using CGG.Application.Specifications;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CGG.Infrastructure.Services;

public class MonobankService : IMonobankService
{
    private readonly IRepository<Transaction> _transactionRepo;
    private readonly IRepository<WebhookLog> _webhookLogRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MonobankSettings _settings;
    private readonly ILogger<MonobankService> _logger;

    public MonobankService(
        IRepository<Transaction> transactionRepo,
        IRepository<WebhookLog> webhookLogRepo,
        IUserRepository userRepo,
        IUnitOfWork unitOfWork,
        IHttpClientFactory httpClientFactory,
        IOptions<MonobankSettings> settings,
        ILogger<MonobankService> logger)
    {
        _transactionRepo = transactionRepo;
        _webhookLogRepo = webhookLogRepo;
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<CreatePaymentResponseDto> CreateInvoiceAsync(Guid userId, decimal amountUAH, CancellationToken ct)
    {
        var orderId = GenerateOrderId();
        var creditsToReceive = ICreditService.CalculateCredits(amountUAH);
        var amountKopecks = (long)Math.Round(amountUAH * 100);

        // Build return URL with orderId so frontend can show result
        var returnUrl = BuildReturnUrl(orderId);

        // Persist pending transaction so webhook can find it
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = amountUAH,
            AmountUAH = amountUAH,
            CreditsGranted = creditsToReceive,
            Type = "purchase",
            Description = $"Поповнення балансу на {creditsToReceive} кредитів",
            OrderId = orderId,
            Status = "pending",
            CreatedAt = DateTime.UtcNow,
        };

        await _transactionRepo.AddAsync(transaction, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Created pending transaction {TransactionId} for user {UserId}, orderId={OrderId}, amountUAH={AmountUAH}, credits={Credits}",
            transaction.Id, userId, orderId, amountUAH, creditsToReceive);

        // Call Monobank API
        var requestBody = new
        {
            amount = amountKopecks,
            ccy = 980, // UAH
            merchantPaymInfo = new
            {
                reference = orderId,
                destination = $"Поповнення кредитів CGG",
                comment = $"Замовлення {orderId}"
            },
            redirectUrl = returnUrl,
            webHookUrl = _settings.WebhookUrl,
            validity = _settings.ValiditySeconds,
        };

        string? paymentUrl = null;
        string? invoiceId = null;

        try
        {
            var client = _httpClientFactory.CreateClient("monobank");
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/merchant/invoice/create", content, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("Monobank API response: status={Status}, body={Body}", response.StatusCode, responseBody);

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                invoiceId = root.TryGetProperty("invoiceId", out var inv) ? inv.GetString() : null;
                paymentUrl = root.TryGetProperty("pageUrl", out var url) ? url.GetString()
                           : root.TryGetProperty("paymentUrl", out var url2) ? url2.GetString()
                           : null;
            }
            else
            {
                _logger.LogError("Monobank API error: {Status} {Body}", response.StatusCode, responseBody);
                throw new InvalidOperationException($"Помилка Monobank API: {response.StatusCode} — {responseBody}");
            }
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call Monobank API for orderId={OrderId}", orderId);
            throw new InvalidOperationException($"Не вдалося створити платіж: {ex.Message}", ex);
        }

        // Update transaction with Monobank response
        transaction.InvoiceId = invoiceId;
        transaction.PaymentUrl = paymentUrl;
        _transactionRepo.Update(transaction);
        await _unitOfWork.SaveChangesAsync(ct);

        return new CreatePaymentResponseDto
        {
            OrderId = orderId,
            PaymentUrl = paymentUrl,
            InvoiceId = invoiceId,
            AmountUAH = amountUAH,
            CreditsToReceive = creditsToReceive,
        };
    }

    public async Task HandleWebhookAsync(MonobankWebhookPayload payload, CancellationToken ct)
    {
        // Extract reference (orderId) from payload
        var reference = payload.MerchantPaymInfo?.Reference;
        var invoiceId = payload.InvoiceId;
        var status = payload.Status?.ToLowerInvariant();

        _logger.LogInformation(
            "Monobank webhook received: reference={Reference}, invoiceId={InvoiceId}, status={Status}",
            reference, invoiceId, status);

        // Persist raw log entry immediately so we never miss a webhook even if processing fails
        var webhookLog = new WebhookLog
        {
            Id = Guid.NewGuid(),
            Source = "monobank",
            ExternalId = invoiceId,
            OrderId = reference,
            Status = status,
            RawBody = JsonSerializer.Serialize(payload),
            ProcessingResult = "ok",
            ReceivedAt = DateTime.UtcNow,
        };
        await _webhookLogRepo.AddAsync(webhookLog, ct);

        if (string.IsNullOrEmpty(reference) && string.IsNullOrEmpty(invoiceId))
        {
            _logger.LogWarning("Monobank webhook missing both reference and invoiceId — ignoring");
            webhookLog.ProcessingResult = "skipped";
            webhookLog.Error = "Missing reference and invoiceId";
            await _unitOfWork.SaveChangesAsync(ct);
            return;
        }

        // Find transaction by orderId (reference)
        Transaction? transaction = null;
        if (!string.IsNullOrEmpty(reference))
        {
            var spec = new TransactionByOrderIdSpec(reference);
            transaction = await _transactionRepo.FirstOrDefaultAsync(spec, ct);
        }

        if (transaction is null && !string.IsNullOrEmpty(invoiceId))
        {
            // Fallback: search by invoiceId
            var spec = new TransactionByInvoiceIdSpec(invoiceId);
            transaction = await _transactionRepo.FirstOrDefaultAsync(spec, ct);
        }

        if (transaction is null)
        {
            _logger.LogError("Monobank webhook: transaction not found for reference={Reference}, invoiceId={InvoiceId}", reference, invoiceId);
            webhookLog.ProcessingResult = "error";
            webhookLog.Error = $"Transaction not found (reference={reference}, invoiceId={invoiceId})";
            await _unitOfWork.SaveChangesAsync(ct);
            throw new InvalidOperationException($"Транзакцію не знайдено (reference={reference}, invoiceId={invoiceId})");
        }

        // Idempotency: skip if already success
        if (transaction.Status == "success")
        {
            _logger.LogInformation("Monobank webhook: transaction {Id} already success — skip", transaction.Id);
            webhookLog.ProcessingResult = "skipped";
            webhookLog.Error = "Already success";
            await _unitOfWork.SaveChangesAsync(ct);
            return;
        }

        // Map status
        var newStatus = status switch
        {
            "success" or "paid" => "success",
            "failure" or "failed" => "failure",
            "expired" => "expired",
            "reversed" => "reversed",
            _ => status ?? "processing"
        };

        transaction.Status = newStatus;
        _transactionRepo.Update(transaction);

        // Credit user on success
        if (newStatus == "success" && transaction.UserId.HasValue)
        {
            var user = await _userRepo.GetByIdAsync(transaction.UserId.Value, ct);
            if (user is null)
            {
                _logger.LogError("Monobank webhook: user {UserId} not found", transaction.UserId);
                webhookLog.ProcessingResult = "error";
                webhookLog.Error = $"User {transaction.UserId} not found";
            }
            else
            {
                var credits = transaction.CreditsGranted > 0
                    ? transaction.CreditsGranted
                    : ICreditService.CalculateCredits(transaction.AmountUAH);

                user.Credits += credits;
                _userRepo.Update(user);

                _logger.LogInformation(
                    "Monobank webhook: credited {Credits} to user {UserId} (orderId={OrderId})",
                    credits, user.Id, transaction.OrderId);
            }
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }

    // ── Verify payment via Monobank status API ──────────────────────────────

    public async Task<string> VerifyPaymentAsync(string invoiceId, CancellationToken ct)
    {
        _logger.LogInformation("Verifying Monobank invoice {InvoiceId} directly via API", invoiceId);

        try
        {
            var client = _httpClientFactory.CreateClient("monobank");
            var response = await client.GetAsync($"/api/merchant/invoice/status?invoiceId={invoiceId}", ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("Monobank invoice status response: {Status} {Body}", response.StatusCode, responseBody);

            if (!response.IsSuccessStatusCode)
                return "unknown";

            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            var rawStatus = root.TryGetProperty("status", out var s) ? s.GetString() : null;
            var reference = root.TryGetProperty("merchantPaymInfo", out var mpi)
                         && mpi.TryGetProperty("reference", out var r) ? r.GetString() : null;
            var amount = root.TryGetProperty("amount", out var a) ? a.GetInt64() : 0;

            // Build fake payload to reuse HandleWebhookAsync logic
            var fakePayload = new MonobankWebhookPayload
            {
                InvoiceId = invoiceId,
                Status = rawStatus,
                Amount = amount,
                MerchantPaymInfo = reference is not null
                    ? new MonobankMerchantPaymInfo { Reference = reference }
                    : null,
            };

            await HandleWebhookAsync(fakePayload, ct);

            return rawStatus switch
            {
                "success" or "paid" => "success",
                "failure" or "failed" => "failure",
                "expired" => "expired",
                "reversed" => "reversed",
                _ => rawStatus ?? "processing"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "VerifyPaymentAsync failed for invoiceId={InvoiceId}", invoiceId);
            return "unknown";
        }
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static string GenerateOrderId()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var code = new char[6];
        var random = Random.Shared;
        for (var i = 0; i < 6; i++)
            code[i] = chars[random.Next(chars.Length)];
        return $"ORD-{new string(code)}";
    }

    private string BuildReturnUrl(string orderId)
    {
        var returnPath = _settings.ReturnUrl;
        if (!returnPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            // relative path — only used in tests; webhook doesn't need absolute URL from here
            return returnPath + (returnPath.Contains('?') ? "&" : "?") + $"orderId={orderId}";
        }
        return returnPath + (returnPath.Contains('?') ? "&" : "?") + $"orderId={orderId}";
    }
}
