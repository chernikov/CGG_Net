namespace CGG.Core.Entities
{
    public class Language
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty; // 'en', 'uk', 'hi'
        public string Name { get; set; } = string.Empty; // 'English', 'Українська', 'हिन्दी'
        public string NativeName { get; set; } = string.Empty; // Native name
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Collections
        public ICollection<SurveyQuestionTranslation> QuestionTranslations { get; set; } = new List<SurveyQuestionTranslation>();
        public ICollection<SurveyQuestionOptionTranslation> OptionTranslations { get; set; } = new List<SurveyQuestionOptionTranslation>();
    }
}
