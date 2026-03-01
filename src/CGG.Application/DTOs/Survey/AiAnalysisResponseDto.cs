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

    /// <summary>Total tokens consumed by this request (for logging / cost tracking).</summary>
    public int? TokensUsed { get; set; }

    /// <summary>Whether the AI call succeeded or there was a recoverable error.</summary>
    public bool Success { get; set; } = true;

    /// <summary>Error message if Success == false.</summary>
    public string? Error { get; set; }
}
