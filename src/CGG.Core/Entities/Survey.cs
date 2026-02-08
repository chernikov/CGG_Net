namespace CGG.Core.Entities
{
    public class Survey
    {
        public Guid Id { get; set; }
        public string SurveyType { get; set; } = string.Empty; // 'classic', 'gaming', 'ab-test'
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        // Configuration
        public bool IsActive { get; set; } = true;
        public bool IsPublic { get; set; } = true; // Allow anonymous users
        
        // Localization
        public Guid DefaultLanguageId { get; set; }
        public Language DefaultLanguage { get; set; } = null!;
        
        // Metadata
        public int Version { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Collections
        public ICollection<SurveyStep> Steps { get; set; } = new List<SurveyStep>();
        public ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
    }
}
