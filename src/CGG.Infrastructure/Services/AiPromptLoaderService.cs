using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.Interfaces;
using CGG.Application.Specifications.Survey;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Services;

/// <summary>
/// Loads AI prompt templates from the database and output-format JSON schemas from disk.
/// </summary>
public class AiPromptLoaderService : IAiPromptLoaderService
{
    private readonly IReadRepository<AiPromptTemplate> _promptRepo;
    private readonly ILogger<AiPromptLoaderService> _logger;

    // Lazy-load format JSON strings (cached after first read)
    private static string? _shortFormatJson;
    private static string? _fullFormatJson;
    private static readonly object _lock = new();

    public AiPromptLoaderService(
        IReadRepository<AiPromptTemplate> promptRepo,
        ILogger<AiPromptLoaderService> logger)
    {
        _promptRepo = promptRepo;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AiPromptTemplate?> LoadPromptForStepAsync(
        string surveyType,
        int stepNumber,
        CancellationToken cancellationToken = default)
    {
        // 1. Try exact match (surveyType + stepNumber)
        var spec = new AiPromptByStepSpecification(surveyType, stepNumber);
        var prompt = await _promptRepo.FirstOrDefaultAsync(spec, cancellationToken);

        if (prompt != null)
        {
            _logger.LogDebug("Loaded prompt template for survey={SurveyType} step={Step} (id={Id})",
                surveyType, stepNumber, prompt.Id);
            return prompt;
        }

        // 2. Fallback: generic template for this step number only
        var fallbackSpec = new AiPromptByStepSpecification(stepNumber);
        var fallback = await _promptRepo.FirstOrDefaultAsync(fallbackSpec, cancellationToken);

        if (fallback != null)
        {
            _logger.LogDebug("Using generic fallback prompt template for step={Step} (id={Id})",
                stepNumber, fallback.Id);
        }
        else
        {
            _logger.LogWarning("No prompt template found for survey={SurveyType} step={Step}. Using hard-coded defaults.",
                surveyType, stepNumber);
        }

        return fallback;
    }

    /// <inheritdoc />
    public string GetOutputFormatJson(string surveyType, int stepNumber, int totalSteps)
    {
        bool isFinalStep = stepNumber >= totalSteps;
        return isFinalStep ? GetFullFormatJson() : GetShortFormatJson();
    }

    // ---------------------------------------------------------------
    // Private helpers
    // ---------------------------------------------------------------

    private static string GetShortFormatJson()
    {
        if (_shortFormatJson != null) return _shortFormatJson;
        lock (_lock) { _shortFormatJson ??= ReadFormatFile("short-profession.json"); }
        return _shortFormatJson;
    }

    private static string GetFullFormatJson()
    {
        if (_fullFormatJson != null) return _fullFormatJson;
        lock (_lock) { _fullFormatJson ??= ReadFormatFile("full-profession.json"); }
        return _fullFormatJson;
    }

    private static string ReadFormatFile(string fileName)
    {
        // Resolved relative to the assembly location so it works in Docker too
        var basePath = AppContext.BaseDirectory;
        var filePath = Path.Combine(basePath, "Data", "Config", "Formats", fileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Output format schema not found: {filePath}");

        return File.ReadAllText(filePath);
    }
}
