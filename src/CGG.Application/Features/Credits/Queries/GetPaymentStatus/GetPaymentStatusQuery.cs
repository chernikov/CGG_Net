using MediatR;

namespace CGG.Application.Features.Credits.Queries.GetPaymentStatus;

public record GetPaymentStatusQuery(string OrderId) : IRequest<PaymentStatusDto?>;

public class PaymentStatusDto
{
    public string OrderId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal AmountUAH { get; set; }
    public int CreditsGranted { get; set; }
    public string? Description { get; set; }
}
