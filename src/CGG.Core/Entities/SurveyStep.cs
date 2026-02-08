namespace CGG.Core.Entities
{
    public class SurveyStep
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; } = null!;
        
        public int StepNumber { get; set; } // 1, 2, 3...
        public Guid QuestionId { get; set; }
        public SurveyQuestion Question { get; set; } = null!;
        
        // AI Configuration
        public string? AiPrompt { get; set; } // AI prompt for this step (if RequiresAiAnalysis)
        public Guid? AiPromptTemplateId { get; set; } // Reference to AiPromptTemplate
        public AiPromptTemplate? AiPromptTemplate { get; set; }
        public bool SkipAiProcessing { get; set; } = false; // Override to skip AI even if question requires it
        
        public bool IsRequired { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
