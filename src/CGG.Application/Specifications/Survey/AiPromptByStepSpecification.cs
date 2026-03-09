using CGG.Core.Entities;

namespace CGG.Application.Specifications.Survey;

/// <summary>
/// Finds the best matching active AiPromptTemplate for a survey step.
/// Matches on category = "survey", given surveyType and stepNumber.
/// </summary>
public class AiPromptByStepSpecification : BaseSpecification<AiPromptTemplate>
{
    /// <summary>
    /// Look for a template that matches both surveyType AND stepNumber.
    /// </summary>
    public AiPromptByStepSpecification(string surveyType, int stepNumber)
        : base(t =>
            t.IsActive &&
            t.Category == "survey" &&
            t.SurveyType == surveyType &&
            t.StepNumber == stepNumber)
    {
    }

    /// <summary>
    /// Fallback: look for a generic template that matches stepNumber only (SurveyType is null/any).
    /// </summary>
    public AiPromptByStepSpecification(int stepNumber)
        : base(t =>
            t.IsActive &&
            t.Category == "survey" &&
            t.SurveyType == null &&
            t.StepNumber == stepNumber)
    {
    }
}
