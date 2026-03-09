using MediatR;

namespace CGG.Application.Features.Credits.Queries.GetCreditsBalance;

public record GetCreditsBalanceQuery(Guid UserId) : IRequest<decimal>;
