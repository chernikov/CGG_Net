using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations;

public class UserSurveyAnswerConfiguration : IEntityTypeConfiguration<UserSurveyAnswer>
{
    public void Configure(EntityTypeBuilder<UserSurveyAnswer> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.QuestionId).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Answer).IsRequired();

        builder.HasOne(e => e.UserSurvey)
            .WithMany(s => s.Answers)
            .HasForeignKey(e => e.UserSurveyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique — guarantees idempotent upsert per question
        builder.HasIndex(e => new { e.UserSurveyId, e.StepNumber, e.QuestionId })
            .IsUnique()
            .HasDatabaseName("IX_UserSurveyAnswers_Survey_Step_Question");
    }
}
