using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class WebhookLogConfiguration : IEntityTypeConfiguration<WebhookLog>
    {
        public void Configure(EntityTypeBuilder<WebhookLog> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Source).HasMaxLength(50).IsRequired();
            builder.Property(e => e.ExternalId).HasMaxLength(100);
            builder.Property(e => e.OrderId).HasMaxLength(50);
            builder.Property(e => e.Status).HasMaxLength(50);
            builder.Property(e => e.RawBody).HasColumnType("nvarchar(max)");
            builder.Property(e => e.ProcessingResult).HasMaxLength(20);
            builder.Property(e => e.Error).HasMaxLength(1000);
            builder.HasIndex(e => e.ExternalId);
            builder.HasIndex(e => e.OrderId);
            builder.HasIndex(e => e.ReceivedAt);
        }
    }
}
