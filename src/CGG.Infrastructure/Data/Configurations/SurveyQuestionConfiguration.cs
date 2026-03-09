using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
    {
        public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.QuestionType);
            builder.HasIndex(e => e.IsActive);
            builder.HasIndex(e => e.RequiresAiAnalysis);
            builder.HasIndex(e => e.Purpose);
        }
    }
}
