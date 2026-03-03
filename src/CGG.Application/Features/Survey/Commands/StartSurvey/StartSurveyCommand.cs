using System;
using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Commands.StartSurvey;

public record StartSurveyCommand(StartSurveyDto Payload, Guid UserId)
    : IRequest<StartSurveyResponseDto>;
