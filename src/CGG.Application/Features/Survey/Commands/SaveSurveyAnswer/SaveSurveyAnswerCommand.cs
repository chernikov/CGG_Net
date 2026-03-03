using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Commands.SaveSurveyAnswer;

public record SaveSurveyAnswerCommand(SaveSurveyAnswerDto Payload) : IRequest<SaveSurveyAnswerResponseDto>;
