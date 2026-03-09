using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            builder.Property(e => e.AmountUAH).HasColumnType("decimal(18,2)");
            builder.Property(e => e.OrderId).HasMaxLength(50);
            builder.Property(e => e.InvoiceId).HasMaxLength(100);
            builder.Property(e => e.PaymentUrl).HasMaxLength(500);
            builder.HasIndex(e => e.OrderId);
        }
    }
}
