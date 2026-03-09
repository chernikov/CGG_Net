using CGG.Application.DTOs.Credits;
using MediatR;

namespace CGG.Application.Features.Credits.Commands.CreateMonobankPayment;

public record CreateMonobankPaymentCommand(Guid UserId, decimal AmountUAH) : IRequest<CreatePaymentResponseDto>;
