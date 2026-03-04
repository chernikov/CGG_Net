using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;
using CGG.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CGG.Infrastructure.Services;

/// <summary>
/// Calls the OpenAI Chat Completions API to analyse a completed survey step.
/// Uses prompt templates from <see cref="IAiPromptLoaderService"/> and
/// returns structured JSON matching the short/full profession schema.
/// </summary>
public class AiSurveyService : IAiSurveyService
{
    private readonly IAiPromptLoaderService _promptLoader;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiSurveyService> _logger;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public AiSurveyService(
        IAiPromptLoaderService promptLoader,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AiSurveyService> logger)
    {
        _promptLoader = promptLoader;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AiAnalysisResponseDto> AnalyzeStepAsync(
        SubmitSurveyStepDto dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // ── 1. Load prompt template ────────────────────────────────────────
            var template = await _promptLoader.LoadPromptForStepAsync(
                dto.SurveyType, dto.StepNumber, cancellationToken);

            if (template is null)
                throw new InvalidOperationException(
                    $"No prompt template found for survey '{dto.SurveyType}' step {dto.StepNumber}");

            var systemPrompt = (template.SystemPrompt
                ?? throw new InvalidOperationException(
                    $"Prompt template for '{dto.SurveyType}' step {dto.StepNumber} has no SystemPrompt"))
                .Replace("{{language}}", GetLanguageInstruction(dto.Language));

            var userTemplate = template.PromptText;

            // ── 2. Build user message ──────────────────────────────────────────
            var answersJson = JsonSerializer.Serialize(dto.Answers, _json);
            var prevResultsJson = dto.PreviousResults.Count > 0
                ? JsonSerializer.Serialize(dto.PreviousResults, _json)
                : "[]";

            var isFinalStep = dto.StepNumber >= dto.TotalSteps;
            var outputFormatJson = _promptLoader.GetOutputFormatJson(
                dto.SurveyType, dto.StepNumber, dto.TotalSteps);

            var userMessage = userTemplate
                .Replace("{{answers}}", answersJson)
                .Replace("{{previousResults}}", prevResultsJson)
                .Replace("{{step}}", dto.StepNumber.ToString())
                .Replace("{{surveyType}}", dto.SurveyType);

            userMessage += $"\n\n---\nIMPORTANT: Respond ONLY with valid JSON that strictly matches this schema:\n{outputFormatJson}";

            // ── 3. Call OpenAI ─────────────────────────────────────────────────
            var model = GetModel(isFinalStep);
            var (resultJson, tokensUsed) = await CallOpenAiAsync(systemPrompt, userMessage, model, cancellationToken);

            _logger.LogInformation(
                "AI step {Step} analysed for survey '{SurveyType}'. Tokens: {Tokens}",
                dto.StepNumber, dto.SurveyType, tokensUsed);

            return new AiAnalysisResponseDto
            {
                StepNumber = dto.StepNumber,
                ResultJson = resultJson,
                OutputFormat = isFinalStep ? "full" : "short",
                TokensUsed = tokensUsed,
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AI analysis failed for survey '{SurveyType}' step {Step}",
                dto.SurveyType, dto.StepNumber);

            return new AiAnalysisResponseDto
            {
                StepNumber = dto.StepNumber,
                ResultJson = "{}",
                Success = false,
                Error = ex.Message
            };
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Private helpers
    // ──────────────────────────────────────────────────────────────────────────

    private async Task<(string content, int tokens)> CallOpenAiAsync(
        string systemPrompt,
        string userMessage,
        string model,
        CancellationToken ct)
    {
        var apiKey = _configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException("OpenAI:ApiKey is not configured");

        var requestBody = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user",   content = userMessage  }
            },
            response_format = new { type = "json_object" },
            temperature = 0.7
        };

        var requestJson = JsonSerializer.Serialize(requestBody, _json);
        using var client = _httpClientFactory.CreateClient("OpenAI");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        using var httpResponse = await client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            new StringContent(requestJson, Encoding.UTF8, "application/json"),
            ct);

        httpResponse.EnsureSuccessStatusCode();

        var responseBody = await httpResponse.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(responseBody);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "{}";

        var tokens = 0;
        if (doc.RootElement.TryGetProperty("usage", out var usage) &&
            usage.TryGetProperty("total_tokens", out var tokensEl))
        {
            tokens = tokensEl.GetInt32();
        }

        return (content, tokens);
    }

    private string GetModel(bool isFinalStep)
    {
        if (isFinalStep)
            return _configuration["OpenAI:MiniModel"] ?? "gpt-4o-mini";
        return _configuration["OpenAI:NanoModel"] ?? "gpt-4o-mini";
    }

    private static string GetLanguageInstruction(string language) => language switch
    {
        "uk" => "Відповідай виключно українською мовою.",
        "hi" => "उत्तर केवल हिंदी में दें।",
        _    => "Respond in English only."
    };

}
