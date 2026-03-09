using CGG.Application.DTOs.Credits;

namespace CGG.Application.Interfaces;

public interface IMonobankService
{
    Task<CreatePaymentResponseDto> CreateInvoiceAsync(Guid userId, decimal amountUAH, CancellationToken ct);
    Task HandleWebhookAsync(MonobankWebhookPayload payload, CancellationToken ct);
}
