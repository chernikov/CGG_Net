using System;
using System.Collections.Generic;

namespace CGG.Application.DTOs.Survey;

public class SurveyDetailDto
{
    public Guid Id { get; set; }
    public string SurveyType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Version { get; set; }
    
    public List<SurveyStepDto> Steps { get; set; } = new();
}

public class SurveyStepDto
{
    public Guid Id { get; set; }
    public int StepNumber { get; set; }
    public bool IsRequired { get; set; }
    public SurveyQuestionDto Question { get; set; } = null!;
}

public class SurveyQuestionDto
{
    public Guid Id { get; set; }
    public string QuestionType { get; set; } = string.Empty;
    public string? PurposeCategory { get; set; }
    
    public List<SurveyTranslationDto> Translations { get; set; } = new();
    public List<SurveyQuestionOptionDto> Options { get; set; } = new();
}

public class SurveyQuestionOptionDto
{
    public Guid Id { get; set; }
    public string? Value { get; set; }
    public int SortOrder { get; set; }
    
    public List<SurveyTranslationDto> Translations { get; set; } = new();
}

public class SurveyTranslationDto
{
    public string LanguageCode { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
