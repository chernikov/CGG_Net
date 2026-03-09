using System.Text.Json;
using System.Text.Json.Serialization;
using CGG.Core.Entities;
using CGG.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Data;

public class PromptSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PromptSeeder> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PromptSeeder(ApplicationDbContext context, ILogger<PromptSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (await _context.AiPromptTemplates.AnyAsync(t => t.SystemPrompt != null, ct))
        {
            _logger.LogDebug("Prompt templates already exist, skipping");
            return;
        }

        var basePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "prompts");

        if (!Directory.Exists(basePath))
            basePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "CGG.Infrastructure", "Data", "SeedData", "prompts");

        if (!Directory.Exists(basePath))
        {
            _logger.LogWarning("Prompts seed directory not found at {Path}", basePath);
            return;
        }

        int count = 0;
        foreach (var file in Directory.GetFiles(basePath, "*.json"))
        {
            var json = await File.ReadAllTextAsync(file, ct);

            var listResult = TryDeserializeList(json);
            List<PromptSeedModel?> models = listResult is not null
                ? listResult.Cast<PromptSeedModel?>().ToList()
                : TryDeserializeSingle(json) is { } single
                    ? [single]
                    : [];

            foreach (var m in models.Where(x => x is not null))
            {
                _context.AiPromptTemplates.Add(new AiPromptTemplate
                {
                    Id = Guid.NewGuid(),
                    Category = m!.Category,
                    SurveyType = m.SurveyType,
                    StepNumber = m.StepNumber,
                    SystemPrompt = m.SystemPrompt,
                    PromptText = m.PromptText,
                    OutputFormat = m.OutputFormat,
                    Model = m.Model is not null && Enum.TryParse<AiModelType>(m.Model, out var modelEnum)
                        ? modelEnum
                        : null,
                    MaxTokens = m.MaxTokens,
                    CreditsCost = m.CreditsCost,
                    IsDefault = m.IsDefault,
                    IsActive = true,
                    Version = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                count++;
            }
        }

        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Seeded {Count} AI prompt templates", count);
    }

    private List<PromptSeedModel>? TryDeserializeList(string json)
    {
        try
        {
            var trimmed = json.TrimStart();
            if (!trimmed.StartsWith('[')) return null;
            return JsonSerializer.Deserialize<List<PromptSeedModel>>(json, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private PromptSeedModel? TryDeserializeSingle(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<PromptSeedModel>(json, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private sealed class PromptSeedModel
    {
        [JsonPropertyName("category")]
        public string Category { get; set; } = "survey";

        [JsonPropertyName("surveyType")]
        public string? SurveyType { get; set; }

        [JsonPropertyName("stepNumber")]
        public int? StepNumber { get; set; }

        [JsonPropertyName("systemPrompt")]
        public string? SystemPrompt { get; set; }

        [JsonPropertyName("promptText")]
        public string PromptText { get; set; } = string.Empty;

        [JsonPropertyName("outputFormat")]
        public string? OutputFormat { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("maxTokens")]
        public int? MaxTokens { get; set; }

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("creditsCost")]
        public decimal CreditsCost { get; set; } = 0;
    }
}
