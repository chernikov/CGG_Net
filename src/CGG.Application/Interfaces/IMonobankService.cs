using CGG.Application.DTOs.Credits;

namespace CGG.Application.Interfaces;

public interface IMonobankService
{
    Task<CreatePaymentResponseDto> CreateInvoiceAsync(Guid userId, decimal amountUAH, CancellationToken ct);
    Task HandleWebhookAsync(MonobankWebhookPayload payload, CancellationToken ct);

    /// <summary>
    /// Directly query Monobank invoice status API and apply result to transaction.
    /// Used to recover pending transactions when webhook was never received.
    /// </summary>
    Task<string> VerifyPaymentAsync(string invoiceId, CancellationToken ct);
}
