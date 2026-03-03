using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations;

public class UserSurveyConfiguration : IEntityTypeConfiguration<UserSurvey>
{
    public void Configure(EntityTypeBuilder<UserSurvey> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.SurveyType).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Language).HasMaxLength(10).IsRequired();
        builder.Property(e => e.Status).HasConversion<int>();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Survey)
            .WithMany()
            .HasForeignKey(e => e.SurveyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.UserId, e.SurveyType });
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.StartedAt);
    }
}
