namespace CGG.Core.Entities
{
    public class PromoCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Credits { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; } = 0;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
