using System;

namespace CGG.Application.DTOs.Survey;

public class SurveySummaryDto
{
    public Guid Id { get; set; }
    public string SurveyType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StepCount { get; set; }
}
