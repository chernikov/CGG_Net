using System;
using System.Collections.Generic;

namespace CGG.Application.DTOs.Survey;

/// <summary>
/// Payload sent from the frontend to persist answers for a single survey step.
/// Sent after every "Наступне" click and on step completion.
/// </summary>
public class SaveSurveyAnswerDto
{
    /// <summary>UserSurvey.Id returned by POST /api/survey/start.</summary>
    public Guid UserSurveyId { get; set; }

    /// <summary>1-based step number.</summary>
    public int StepNumber { get; set; }

    /// <summary>Answers collected so far in this step.</summary>
    public List<StepAnswerDto> Answers { get; set; } = new();
}
