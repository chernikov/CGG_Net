using CGG.Application.DTOs.Transaction;
using CGG.Application.Specifications;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.Credits.Queries.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<TransactionDto>>
{
    private readonly IReadRepository<Transaction> _transactionRepo;

    public GetTransactionsQueryHandler(IReadRepository<Transaction> transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    public async Task<List<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new TransactionsByUserIdSpec(request.UserId, request.Page, request.PageSize);
        var transactions = await _transactionRepo.ListAsync(spec, cancellationToken);

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            AmountUAH = t.AmountUAH,
            CreditsGranted = t.CreditsGranted,
            Type = t.Type,
            Description = t.Description,
            Status = t.Status,
            OrderId = t.OrderId,
            PaymentUrl = t.PaymentUrl,
            CreatedAt = t.CreatedAt,
        }).ToList();
    }
}
