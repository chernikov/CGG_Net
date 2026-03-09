namespace CGG.Application.Interfaces;

public interface ICreditService
{
    Task<decimal> GetStepCreditsCostAsync(string surveyType, int stepNumber, CancellationToken ct);
    Task<decimal> DeductCreditsAsync(Guid userId, decimal amount, string description, CancellationToken ct);
    Task<decimal> AddCreditsAsync(Guid userId, int credits, string description, CancellationToken ct);

    /// <summary>
    /// Calculates credits to grant: 1:1 for amounts &lt; 300 UAH, +40% bonus for amounts &gt;= 300 UAH.
    /// </summary>
    static int CalculateCredits(decimal amountUAH)
    {
        if (amountUAH >= 300m)
            return (int)Math.Floor(amountUAH * 1.4m);
        return (int)Math.Floor(amountUAH);
    }
}
