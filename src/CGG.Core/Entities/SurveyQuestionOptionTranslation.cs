namespace CGG.Core.Entities
{
    public class SurveyQuestionOptionTranslation
    {
        public Guid Id { get; set; }
        public Guid OptionId { get; set; }
        public SurveyQuestionOption Option { get; set; } = null!;
        
        public Guid LanguageId { get; set; }
        public Language Language { get; set; } = null!;
        
        public string Text { get; set; } = string.Empty; // Option text in this language
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
