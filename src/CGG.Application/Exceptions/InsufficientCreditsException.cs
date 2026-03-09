namespace CGG.Application.Exceptions;

public class InsufficientCreditsException : Exception
{
    public decimal Required { get; }
    public decimal Available { get; }

    public InsufficientCreditsException(decimal required, decimal available)
        : base($"Insufficient credits. Required: {required}, available: {available}")
    {
        Required = required;
        Available = available;
    }
}
