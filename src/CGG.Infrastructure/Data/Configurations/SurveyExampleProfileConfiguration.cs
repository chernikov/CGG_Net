using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations;

public class SurveyExampleProfileConfiguration : IEntityTypeConfiguration<SurveyExampleProfile>
{
    public void Configure(EntityTypeBuilder<SurveyExampleProfile> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.SurveyType);
        builder.HasIndex(e => new { e.SurveyType, e.Slug }).IsUnique();
        builder.Property(e => e.SurveyType).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Slug).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Icon).HasMaxLength(10).IsRequired();
        builder.Property(e => e.NameUk).HasMaxLength(200).IsRequired();
        builder.Property(e => e.NameEn).HasMaxLength(200).IsRequired();
    }
}
