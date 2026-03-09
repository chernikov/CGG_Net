using CGG.Core.Entities;

namespace CGG.Application.Specifications;

public class TransactionByInvoiceIdSpec : BaseSpecification<Transaction>
{
    public TransactionByInvoiceIdSpec(string invoiceId)
        : base(t => t.InvoiceId == invoiceId)
    {
    }
}
