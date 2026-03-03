using System;

namespace CGG.Application.DTOs.Survey;

/// <summary>Payload to start a new survey pass.</summary>
public class StartSurveyDto
{
    /// <summary>Survey definition ID returned by GET /api/survey/item.</summary>
    public Guid SurveyId { get; set; }

    /// <summary>"classic" | "gaming" | "ab-test" etc.</summary>
    public string SurveyType { get; set; } = string.Empty;

    /// <summary>UI language code: "uk", "en", "hi".</summary>
    public string Language { get; set; } = "uk";
}

/// <summary>Returned after starting a survey.</summary>
public class StartSurveyResponseDto
{
    public Guid UserSurveyId { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
}
