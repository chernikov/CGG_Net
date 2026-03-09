using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Credits.Queries.GetCreditsBalance;

public class GetCreditsBalanceQueryHandler : IRequestHandler<GetCreditsBalanceQuery, decimal>
{
    private readonly IUserRepository _userRepo;

    public GetCreditsBalanceQueryHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<decimal> Handle(GetCreditsBalanceQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new InvalidOperationException($"User {request.UserId} not found");

        return user.Credits;
    }
}
