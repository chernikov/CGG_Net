using CGG.Application.DTOs.Credits;
using MediatR;

namespace CGG.Application.Features.Credits.Commands.HandleMonobankWebhook;

public record HandleMonobankWebhookCommand(MonobankWebhookPayload Payload) : IRequest<Unit>;
