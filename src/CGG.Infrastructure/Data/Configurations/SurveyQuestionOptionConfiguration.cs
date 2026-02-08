using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyQuestionOptionConfiguration : IEntityTypeConfiguration<SurveyQuestionOption>
    {
        public void Configure(EntityTypeBuilder<SurveyQuestionOption> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.QuestionId, e.SortOrder });
            
            builder.HasOne(e => e.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
