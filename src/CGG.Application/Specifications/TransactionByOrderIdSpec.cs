using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class TransactionByOrderIdSpec : BaseSpecification<Transaction>
{
    public TransactionByOrderIdSpec(string orderId)
        : base(t => t.OrderId == orderId)
    {
    }
}
