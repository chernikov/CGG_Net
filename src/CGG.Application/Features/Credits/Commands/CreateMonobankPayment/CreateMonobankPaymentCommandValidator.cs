using FluentValidation;

namespace CGG.Application.Features.Credits.Commands.CreateMonobankPayment;

public class CreateMonobankPaymentCommandValidator : AbstractValidator<CreateMonobankPaymentCommand>
{
    public CreateMonobankPaymentCommandValidator()
    {
        RuleFor(x => x.AmountUAH)
            .GreaterThanOrEqualTo(1m)
            .WithMessage("Мінімальна сума поповнення — 1 грн.");

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
