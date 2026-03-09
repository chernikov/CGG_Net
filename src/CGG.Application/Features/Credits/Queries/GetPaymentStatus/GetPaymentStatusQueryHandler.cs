using CGG.Application.Specifications;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Credits.Queries.GetPaymentStatus;

public class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, PaymentStatusDto?>
{
    private readonly IReadRepository<Transaction> _transactionRepo;

    public GetPaymentStatusQueryHandler(IReadRepository<Transaction> transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    public async Task<PaymentStatusDto?> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        var spec = new TransactionByOrderIdSpec(request.OrderId);
        var transaction = await _transactionRepo.FirstOrDefaultAsync(spec, cancellationToken);

        if (transaction is null)
            return null;

        return new PaymentStatusDto
        {
            OrderId = transaction.OrderId ?? request.OrderId,
            Status = transaction.Status,
            AmountUAH = transaction.AmountUAH,
            CreditsGranted = transaction.CreditsGranted,
            Description = transaction.Description,
        };
    }
}
