namespace CGG.Application.DTOs.Credits;

public class CreatePaymentResponseDto
{
    public string OrderId { get; set; } = string.Empty;
    public string? PaymentUrl { get; set; }
    public string? InvoiceId { get; set; }
    public decimal AmountUAH { get; set; }
    public int CreditsToReceive { get; set; }
}
