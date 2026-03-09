namespace CGG.Application.DTOs.Survey;

/// <summary>
/// Response returned by the AI analysis endpoint after processing a survey step.
/// The frontend stores this in the session and uses it to display intermediate results
/// and to pass as context to subsequent step analyses.
/// </summary>
public class AiAnalysisResponseDto
{
    /// <summary>Step number that was analysed.</summary>
    public int StepNumber { get; set; }

    /// <summary>
    /// AI-generated result as a JSON string.
    /// Format depends on OutputFormat:
    ///   "short" → { matches: [{ title, matchPercentage }] }
    ///   "full"  → { matches: [...full profession detail...], overallPersonalityProfile }
    /// </summary>
    public string ResultJson { get; set; } = string.Empty;

    /// <summary>"short" for intermediate steps, "full" for the final step.</summary>
    public string OutputFormat { get; set; } = "short";

    // ── AI call metadata (used to populate AiLog) ─────────────────────────

    /// <summary>Prompt template ID that was used.</summary>
    public Guid? PromptTemplateId { get; set; }

    /// <summary>AI provider, e.g. "openai".</summary>
    public string? Provider { get; set; }

    /// <summary>Model name, e.g. "gpt-5-mini".</summary>
    public string? Model { get; set; }

    /// <summary>System prompt sent to the model (after language injection).</summary>
    public string? SystemPrompt { get; set; }

    /// <summary>Full user message sent to the model (including output-format schema).</summary>
    public string? PromptText { get; set; }

    /// <summary>Raw answers JSON sent by the user for this step.</summary>
    public string? UserInput { get; set; }

    // ── Token counts ──────────────────────────────────────────────────────

    /// <summary>Total tokens consumed by this request (for logging / cost tracking).</summary>
    public int? TokensUsed { get; set; }

    /// <summary>Input / prompt tokens.</summary>
    public int? PromptTokens { get; set; }

    /// <summary>Output / completion tokens.</summary>
    public int? CompletionTokens { get; set; }

    /// <summary>End-to-end processing time in milliseconds.</summary>
    public int? ProcessingTimeMs { get; set; }

    // ── Status ────────────────────────────────────────────────────────────

    /// <summary>Whether the AI call succeeded or there was a recoverable error.</summary>
    public bool Success { get; set; } = true;

    /// <summary>Error message if Success == false.</summary>
    public string? Error { get; set; }

    /// <summary>Error code if Success == false (e.g. HTTP status code string).</summary>
    public string? ErrorCode { get; set; }

    /// <summary>User's remaining credits balance after this analysis (null if no credits were charged).</summary>
    public decimal? CreditsRemaining { get; set; }
}
