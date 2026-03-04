namespace CGG.Core.Entities;

public class SurveyExampleProfile
{
    public Guid Id { get; set; }
    public required string SurveyType { get; set; }
    public required string Slug { get; set; }
    public required string Icon { get; set; }
    public required string NameUk { get; set; }
    public required string NameEn { get; set; }
    public int SortOrder { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SurveyExampleAnswer> Answers { get; set; } = new List<SurveyExampleAnswer>();
}
