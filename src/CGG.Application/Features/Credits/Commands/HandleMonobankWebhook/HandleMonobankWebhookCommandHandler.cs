using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Credits.Commands.HandleMonobankWebhook;

public class HandleMonobankWebhookCommandHandler : IRequestHandler<HandleMonobankWebhookCommand, Unit>
{
    private readonly IMonobankService _monobankService;

    public HandleMonobankWebhookCommandHandler(IMonobankService monobankService)
    {
        _monobankService = monobankService;
    }

    public async Task<Unit> Handle(HandleMonobankWebhookCommand request, CancellationToken cancellationToken)
    {
        await _monobankService.HandleWebhookAsync(request.Payload, cancellationToken);
        return Unit.Value;
    }
}
