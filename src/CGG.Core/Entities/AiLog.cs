namespace CGG.Core.Entities
{
    public class AiLog
    {
        public Guid Id { get; set; }
        
        // Request context
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        
        public Guid? MemberId { get; set; }
        public Member? Member { get; set; }
        
        public Guid? SurveyResultId { get; set; }
        public SurveyResult? SurveyResult { get; set; }
        
        public Guid? PromptTemplateId { get; set; }
        public AiPromptTemplate? PromptTemplate { get; set; }
        
        // AI Request details
        public string RequestType { get; set; } = string.Empty; // 'survey_step', 'recommendation', 'custom', etc.
        public string? Provider { get; set; } // 'openai', 'azure-openai', 'anthropic'
        public string? Model { get; set; } // 'gpt-4', 'gpt-3.5-turbo', etc.
        
        // Step info (for surveys)
        public int? StepNumber { get; set; }
        
        // Prompt and response
        public string? PromptText { get; set; } // The actual prompt sent
        public string? SystemPrompt { get; set; } // System message
        public string? UserInput { get; set; } // User's input/answers
        public string? Response { get; set; } // AI response (JSON or text)
        
        // Processing details
        public int? TokensUsed { get; set; }
        public int? PromptTokens { get; set; }
        public int? CompletionTokens { get; set; }
        public int? ProcessingTimeMs { get; set; }
        
        // Status
        public string Status { get; set; } = "success"; // 'success', 'error', 'timeout'
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        
        // Cost tracking
        public decimal? CostUsd { get; set; }
        
        // Metadata
        public string? Metadata { get; set; } // JSON for additional data
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
