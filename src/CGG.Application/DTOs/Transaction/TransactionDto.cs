namespace CGG.Application.DTOs.Transaction;

public class TransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountUAH { get; set; }
    public int CreditsGranted { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? OrderId { get; set; }
    public string? PaymentUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
