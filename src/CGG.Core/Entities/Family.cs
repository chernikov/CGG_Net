namespace CGG.Core.Entities
{
    public class Family
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
    }
}
