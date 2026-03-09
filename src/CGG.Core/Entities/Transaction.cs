namespace CGG.Core.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public decimal Amount { get; set; }
        public decimal AmountUAH { get; set; }
        public int CreditsGranted { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PaymentId { get; set; }
        
        // Monobank payment fields
        public string? OrderId { get; set; }
        public string? InvoiceId { get; set; }
        public string? PaymentUrl { get; set; }
        
        public string Status { get; set; } = "completed";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
