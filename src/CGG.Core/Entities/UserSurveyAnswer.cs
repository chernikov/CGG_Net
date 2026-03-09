namespace CGG.Core.Entities;

/// <summary>
/// Stores a single question answer within a UserSurvey.
/// The combination (UserSurveyId, StepNumber, QuestionId) is unique —
/// saving the same question twice upserts in place.
/// </summary>
public class UserSurveyAnswer
{
    public Guid Id { get; set; }

    public Guid UserSurveyId { get; set; }
    public UserSurvey UserSurvey { get; set; } = null!;

    /// <summary>1-based step number this answer belongs to.</summary>
    public int StepNumber { get; set; }

    /// <summary>SurveyQuestion.Id (UUID string from the survey definition).</summary>
    public string QuestionId { get; set; } = string.Empty;

    /// <summary>Question text at time of answering (locale-resolved, for readability).</summary>
    public string? QuestionText { get; set; }

    /// <summary>
    /// Raw answer value.
    /// Single-choice / text / scale / rating → plain string.
    /// Multiple-choice → JSON array string e.g. '["A","B"]'.
    /// </summary>
    public string Answer { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
