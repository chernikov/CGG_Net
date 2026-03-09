using CGG.Application.DTOs.Credits;
using CGG.Application.Interfaces;
using MediatR;

namespace CGG.Application.Features.Credits.Commands.CreateMonobankPayment;

public class CreateMonobankPaymentCommandHandler : IRequestHandler<CreateMonobankPaymentCommand, CreatePaymentResponseDto>
{
    private readonly IMonobankService _monobankService;

    public CreateMonobankPaymentCommandHandler(IMonobankService monobankService)
    {
        _monobankService = monobankService;
    }

    public Task<CreatePaymentResponseDto> Handle(CreateMonobankPaymentCommand request, CancellationToken cancellationToken)
    {
        return _monobankService.CreateInvoiceAsync(request.UserId, request.AmountUAH, cancellationToken);
    }
}
