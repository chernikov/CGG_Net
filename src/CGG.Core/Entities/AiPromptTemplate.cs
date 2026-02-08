namespace CGG.Core.Entities
{
    public class AiPromptTemplate
    {
        public Guid Id { get; set; }
        public string PromptType { get; set; } = string.Empty; // 'classic_step1', 'gaming_step2', 'ai-recommendation', etc.
        public string Category { get; set; } = string.Empty; // 'survey', 'recommendation', 'preview', 'feedback'
        
        // Prompt content
        public string PromptText { get; set; } = string.Empty;
        public string? SystemPrompt { get; set; } // System message for AI
        public string? Description { get; set; } // What this prompt does
        
        // Configuration
        public string? Model { get; set; } // 'gpt-4', 'gpt-3.5-turbo', etc.
        public double? Temperature { get; set; }
        public int? MaxTokens { get; set; }
        
        // Versioning
        public int Version { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false; // Is this the default active version
        
        // Metadata
        public string? Tags { get; set; } // JSON array: ["survey", "step1"]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CreatedByUserId { get; set; }
        public User? CreatedBy { get; set; }

        // Collections
        public ICollection<SurveyStep> SurveySteps { get; set; } = new List<SurveyStep>();
        public ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();
    }
}
