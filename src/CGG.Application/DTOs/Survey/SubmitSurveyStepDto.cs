using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CGG.Application.DTOs.Survey;

/// <summary>
/// Payload sent from the frontend after the user completes a survey step.
/// The backend uses this to build the AI prompt and return an analysis result.
/// </summary>
public class SubmitSurveyStepDto
{
    /// <summary>Survey type identifier: "classic", "gaming", "ab-test", etc.</summary>
    public string SurveyType { get; set; } = string.Empty;

    /// <summary>1-based step number being submitted.</summary>
    public int StepNumber { get; set; }

    /// <summary>Total number of steps in the survey (used to choose output format).</summary>
    public int TotalSteps { get; set; }

    /// <summary>UI language code: "uk", "en", "hi".</summary>
    public string Language { get; set; } = "uk";

    /// <summary>Answers provided by the user for this step.</summary>
    public List<StepAnswerDto> Answers { get; set; } = new();

    /// <summary>
    /// AI results from previous steps – passed as context for accumulating career insights.
    /// Empty for step 1.
    /// </summary>
    public List<PreviousAiResultDto> PreviousResults { get; set; } = new();
}

/// <summary>A single question answer within a step.</summary>
public class StepAnswerDto
{
    /// <summary>Question ID (UUID string matching SurveyQuestion.Id).</summary>
    public string QuestionId { get; set; } = string.Empty;

    /// <summary>Question text at the time of answering (for prompt context).</summary>
    public string? QuestionText { get; set; }

    /// <summary>
    /// Answer value.
    /// - For single-choice / text / textarea / scale / rating / email / number → string
    /// - For multiple-choice → JSON array string: ["optionA","optionB"]
    /// </summary>
    public string Answer { get; set; } = string.Empty;
}

/// <summary>AI result from a previously completed step, included for context.</summary>
public class PreviousAiResultDto
{
    public int Step { get; set; }

    /// <summary>Raw JSON string returned by OpenAI for this step.</summary>
    public string ResultJson { get; set; } = string.Empty;
}
