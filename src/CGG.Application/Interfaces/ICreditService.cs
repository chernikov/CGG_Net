namespace CGG.Application.Interfaces;

public interface ICreditService
{
    Task<decimal> GetStepCreditsCostAsync(string surveyType, int stepNumber, CancellationToken ct);
    Task<decimal> DeductCreditsAsync(Guid userId, decimal amount, string description, CancellationToken ct);
}
