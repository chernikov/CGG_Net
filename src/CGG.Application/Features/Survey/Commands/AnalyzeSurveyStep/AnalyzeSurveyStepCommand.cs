using System;
using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Commands.AnalyzeSurveyStep;

/// <summary>
/// MediatR command that triggers AI analysis for a completed survey step.
/// </summary>
public record AnalyzeSurveyStepCommand(SubmitSurveyStepDto Payload, Guid? UserId = null)
    : IRequest<AiAnalysisResponseDto>;
