namespace CGG.Core.Entities
{
    public class SurveyResult
    {
        public Guid Id { get; set; }
        
        // Reference to survey template
        public Guid? SurveyId { get; set; }
        public Survey? Survey { get; set; }
        
        // v1 fields (legacy)
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        
        // v2 fields
        public Guid? MemberId { get; set; }
        public Member? Member { get; set; }
        
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public string SurveyType { get; set; } = string.Empty; // 'classic', 'gaming', 'ab-test'
        
        // Language reference
        public Guid LanguageId { get; set; }
        public Language Language { get; set; } = null!;
        
        public string? CurrentStep { get; set; } // Can be number or 'feedback'/'done'
        
        // User responses and AI results
        public string? Steps { get; set; } // JSON array of step responses and AI analysis
        public string? Results { get; set; } // JSON array of profession matches with scores
        
        // AI processing info
        public int? TokensUsed { get; set; }
        public int? ProcessingTimeMs { get; set; }
        
        public string? Metadata { get; set; } // JSON object for additional data
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; } // When CurrentStep = 'done'

        // Collections
        public ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();
    }
}
