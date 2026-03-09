using CGG.Core.Enums;

namespace CGG.Core.Entities
{
    public class AiPromptTemplate
    {
        public Guid Id { get; set; }
        public string Category { get; set; } = string.Empty; // 'survey', 'recommendation', 'preview', 'feedback'
        
        // Survey specific fields
        public string? SurveyType { get; set; } // 'classic', 'gaming', 'ab-test', etc.
        public int? StepNumber { get; set; } // 1, 2, 3...
        public string? OutputFormat { get; set; } // 'short', 'full', etc.

        // Prompt content
        public string PromptText { get; set; } = string.Empty;
        public string? SystemPrompt { get; set; } // System message for AI
        
        // Configuration
        public AiModelType? Model { get; set; }
        public int? MaxTokens { get; set; }
        public decimal CreditsCost { get; set; } = 0;

        // Versioning
        public int Version { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false; // Is this the default active version
        
        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Collections
        public ICollection<SurveyStep> SurveySteps { get; set; } = new List<SurveyStep>();
        public ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();
    }
}
