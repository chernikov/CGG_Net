namespace CGG.Core.Entities
{
    public class SurveyQuestionTranslation
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public SurveyQuestion Question { get; set; } = null!;
        
        public Guid LanguageId { get; set; }
        public Language Language { get; set; } = null!;
        
        public string Text { get; set; } = string.Empty; // Question text in this language
        public string? Description { get; set; } // Optional description/hint
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
