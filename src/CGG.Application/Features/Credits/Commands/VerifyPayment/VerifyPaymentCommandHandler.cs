using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Credits.Commands.VerifyPayment;

public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, string>
{
    private readonly IMonobankService _monobankService;

    public VerifyPaymentCommandHandler(IMonobankService monobankService)
    {
        _monobankService = monobankService;
    }

    public Task<string> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        => _monobankService.VerifyPaymentAsync(request.InvoiceId, cancellationToken);
}
