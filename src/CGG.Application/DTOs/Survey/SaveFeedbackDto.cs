namespace CGG.Application.DTOs.Survey;

public class SaveFeedbackDto
{
    public required Guid UserSurveyId { get; set; }
    public int? Rating { get; set; }
    public string? Comment { get; set; }
}

public class SaveFeedbackResponseDto
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}
