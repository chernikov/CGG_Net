using System.Text.Json.Serialization;

namespace CGG.Infrastructure.Data.SeedData
{
    public class SurveyStepSeedModel
    {
        [JsonPropertyName("step")]
        public int Step { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public Dictionary<string, string> Title { get; set; } = new();

        [JsonPropertyName("description")]
        public Dictionary<string, string> Description { get; set; } = new();

        [JsonPropertyName("questions")]
        public List<SurveyQuestionSeedModel> Questions { get; set; } = new();

        [JsonPropertyName("systemPrompt")]
        public string? SystemPrompt { get; set; }

        [JsonPropertyName("userPromptTemplate")]
        public string? UserPromptTemplate { get; set; }

        [JsonPropertyName("aiPrompt")]
        public AiPromptSeedModel? AiPrompt { get; set; }

        [JsonPropertyName("outputFormat")]
        public OutputFormatSeedModel? OutputFormat { get; set; }
    }

    public class OutputFormatSeedModel
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("formatFile")]
        public string? FormatFile { get; set; }
    }

    public class AiPromptSeedModel
    {
        [JsonPropertyName("systemPrompt")]
        public string? SystemPrompt { get; set; }

        [JsonPropertyName("userPromptTemplate")]
        public string? UserPromptTemplate { get; set; }
    }

    public class SurveyQuestionSeedModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public Dictionary<string, string> Text { get; set; } = new();

        [JsonPropertyName("options")]
        public Dictionary<string, List<string>>? Options { get; set; }

        [JsonPropertyName("purpose")]
        public string? Purpose { get; set; }
    }
}
