using System.Text.Json;

namespace CGG.Application.DTOs.SurveyExample;

public class SurveyExampleProfileDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string NameUk { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public Dictionary<string, JsonElement> Answers { get; set; } = new();
}
