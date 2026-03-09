using CGG.Application.DTOs.Transaction;
using MediatR;

namespace CGG.Application.Features.Credits.Queries.GetTransactions;

public record GetTransactionsQuery(Guid UserId, int Page, int PageSize) : IRequest<List<TransactionDto>>;
