using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class TransactionsByUserIdSpec : BaseSpecification<Transaction>
{
    public TransactionsByUserIdSpec(Guid userId, int page, int pageSize)
        : base(t => t.UserId == userId)
    {
        ApplyOrderByDescending(t => t.CreatedAt);
        ApplyPaging((page - 1) * pageSize, pageSize);
    }
}
