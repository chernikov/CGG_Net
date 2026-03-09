using CGG.Application.Exceptions;
using CGG.Application.Interfaces;
using CGG.Core.Entities;
using CGG.Core.Interfaces;

namespace CGG.Infrastructure.Services;

public class CreditService : ICreditService
{
    private readonly IAiPromptLoaderService _promptLoader;
    private readonly IUserRepository _userRepo;
    private readonly IRepository<Transaction> _transactionRepo;

    public CreditService(
        IAiPromptLoaderService promptLoader,
        IUserRepository userRepo,
        IRepository<Transaction> transactionRepo)
    {
        _promptLoader = promptLoader;
        _userRepo = userRepo;
        _transactionRepo = transactionRepo;
    }

    public async Task<decimal> GetStepCreditsCostAsync(string surveyType, int stepNumber, CancellationToken ct)
    {
        var template = await _promptLoader.LoadPromptForStepAsync(surveyType, stepNumber, ct);
        return template?.CreditsCost ?? 0;
    }

    public async Task<decimal> DeductCreditsAsync(Guid userId, decimal amount, string description, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(userId, ct)
            ?? throw new InvalidOperationException($"User {userId} not found");

        if (user.Credits < amount)
            throw new InsufficientCreditsException(amount, user.Credits);

        user.Credits -= amount;
        _userRepo.Update(user);

        await _transactionRepo.AddAsync(new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = -amount,
            Type = "ai_analysis",
            Description = description,
            Status = "completed",
            CreatedAt = DateTime.UtcNow,
        }, ct);

        return user.Credits;
    }

    public async Task<decimal> AddCreditsAsync(Guid userId, int credits, string description, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(userId, ct)
            ?? throw new InvalidOperationException($"User {userId} not found");

        user.Credits += credits;
        _userRepo.Update(user);

        await _transactionRepo.AddAsync(new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = credits,
            CreditsGranted = credits,
            Type = "purchase",
            Description = description,
            Status = "completed",
            CreatedAt = DateTime.UtcNow,
        }, ct);

        return user.Credits;
    }
}
