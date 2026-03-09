using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Commands.SaveFeedback;

public record SaveFeedbackCommand(SaveFeedbackDto Payload) : IRequest<SaveFeedbackResponseDto>;
