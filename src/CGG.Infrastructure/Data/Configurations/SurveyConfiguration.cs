using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.SurveyType);
            builder.HasIndex(e => e.IsActive);
            builder.HasIndex(e => new { e.SurveyType, e.Version }).IsUnique();
            
            builder.HasOne(e => e.DefaultLanguage)
                .WithMany()
                .HasForeignKey(e => e.DefaultLanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
