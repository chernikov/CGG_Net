namespace CGG.Core.Entities
{
    public class SurveyQuestionOption
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public SurveyQuestion Question { get; set; } = null!;
        
        public int SortOrder { get; set; } // Order of options
        public string? Value { get; set; } // Optional value for processing
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Collections
        public ICollection<SurveyQuestionOptionTranslation> Translations { get; set; } = new List<SurveyQuestionOptionTranslation>();
    }
}
