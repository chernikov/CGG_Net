using System.Text.Json;
using System.Text.Json.Serialization;

namespace CGG.Infrastructure.Data.SeedData;

public class SurveyExampleProfileSeedModel
{
    [JsonPropertyName("slug")]      public string Slug { get; set; } = string.Empty;
    [JsonPropertyName("icon")]      public string Icon { get; set; } = string.Empty;
    [JsonPropertyName("nameUk")]    public string NameUk { get; set; } = string.Empty;
    [JsonPropertyName("nameEn")]    public string NameEn { get; set; } = string.Empty;
    [JsonPropertyName("sortOrder")] public int SortOrder { get; set; } = 1;
    [JsonPropertyName("answers")]   public JsonElement Answers { get; set; }
}
