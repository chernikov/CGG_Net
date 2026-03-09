using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations;

public class SurveyExampleAnswerConfiguration : IEntityTypeConfiguration<SurveyExampleAnswer>
{
    public void Configure(EntityTypeBuilder<SurveyExampleAnswer> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Purpose).HasMaxLength(200).IsRequired();
        builder.Property(e => e.ValueJson).HasColumnType("nvarchar(max)").IsRequired();

        builder.HasOne(e => e.Profile)
            .WithMany(p => p.Answers)
            .HasForeignKey(e => e.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.ProfileId, e.Purpose }).IsUnique();
    }
}
