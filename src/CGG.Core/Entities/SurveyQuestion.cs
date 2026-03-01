namespace CGG.Core.Entities
{
    public class SurveyQuestion
    {
        public Guid Id { get; set; }
        public string QuestionType { get; set; } = "single-choice"; // 'single-choice', 'multiple-choice', 'text', 'scale', 'rating', 'feedback'
        
        // AI Processing
        public bool RequiresAiAnalysis { get; set; } = true; // false for UI/UX feedback questions
        public string? PurposeCategory { get; set; } // 'career-analysis', 'ui-feedback', 'a-b-test', 'user-satisfaction'
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Step relationship (many questions per step)
        public Guid? StepId { get; set; }
        public int SortOrder { get; set; } = 1;
        public SurveyStep? Step { get; set; }

        // Collections
        public ICollection<SurveyQuestionOption> Options { get; set; } = new List<SurveyQuestionOption>();
        public ICollection<SurveyQuestionTranslation> Translations { get; set; } = new List<SurveyQuestionTranslation>();
    }
}
