using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class AiPromptTemplateConfiguration : IEntityTypeConfiguration<AiPromptTemplate>
    {
        public void Configure(EntityTypeBuilder<AiPromptTemplate> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.PromptType);
            builder.HasIndex(e => e.Category);
            builder.HasIndex(e => e.IsActive);
            builder.HasIndex(e => e.IsDefault);
            builder.HasIndex(e => new { e.PromptType, e.Version });
            
            builder.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedPromptTemplates)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
