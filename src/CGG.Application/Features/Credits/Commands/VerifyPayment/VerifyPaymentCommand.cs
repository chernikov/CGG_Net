using MediatR;

namespace CGG.Application.Features.Credits.Commands.VerifyPayment;

/// <summary>
/// Queries Monobank invoice status API directly and applies result.
/// Used when webhook was never received (e.g. wrong webhook URL in prior config).
/// Returns the updated transaction status string.
/// </summary>
public record VerifyPaymentCommand(string InvoiceId) : IRequest<string>;
