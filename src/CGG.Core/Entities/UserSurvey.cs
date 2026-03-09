namespace CGG.Core.Entities;

/// <summary>
/// Represents a single pass of a user through a survey.
/// When a user starts the same survey type again, the previous record is marked Outdated.
/// </summary>
public class UserSurvey
{
    public Guid Id { get; set; }

    // The user who is taking the survey (required – no anonymous)
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Survey definition reference
    public Guid? SurveyId { get; set; }
    public Survey? Survey { get; set; }

    /// <summary>"classic" | "gaming" | "ab-test" etc.</summary>
    public string SurveyType { get; set; } = string.Empty;

    /// <summary>UI language code: "uk", "en", "hi".</summary>
    public string Language { get; set; } = string.Empty;

    public UserSurveyStatus Status { get; set; } = UserSurveyStatus.InProgress;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public int? FeedbackRating { get; set; }
    public string? FeedbackComment { get; set; }

    // Navigation
    public ICollection<UserSurveyAnswer> Answers { get; set; } = new List<UserSurveyAnswer>();
    public ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();
}
