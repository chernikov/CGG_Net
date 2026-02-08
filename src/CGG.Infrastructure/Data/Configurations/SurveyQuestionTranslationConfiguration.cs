using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyQuestionTranslationConfiguration : IEntityTypeConfiguration<SurveyQuestionTranslation>
    {
        public void Configure(EntityTypeBuilder<SurveyQuestionTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.QuestionId, e.LanguageId }).IsUnique();
            
            builder.HasOne(e => e.Question)
                .WithMany(q => q.Translations)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(e => e.Language)
                .WithMany(l => l.QuestionTranslations)
                .HasForeignKey(e => e.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
