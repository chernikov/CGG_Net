namespace CGG.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserRole Role { get; set; }
        public decimal Credits { get; set; } = 0;
        public bool EmailConfirmed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public Guid? MemberId { get; set; }
        public Member? Member { get; set; }
        
        public Guid? SchoolId { get; set; }
        public School? School { get; set; }

        // Collections
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
        public ICollection<AIRecommendation> AIRecommendations { get; set; } = new List<AIRecommendation>();
        public ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();
        public ICollection<AiPromptTemplate> CreatedPromptTemplates { get; set; } = new List<AiPromptTemplate>();
    }
}
