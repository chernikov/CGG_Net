using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class SurveyStepConfiguration : IEntityTypeConfiguration<SurveyStep>
    {
        public void Configure(EntityTypeBuilder<SurveyStep> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.SurveyId, e.StepNumber }).IsUnique();
            
            builder.HasOne(e => e.Survey)
                .WithMany(s => s.Steps)
                .HasForeignKey(e => e.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.Questions)
                .WithOne(q => q.Step)
                .HasForeignKey(q => q.StepId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(e => e.AiPromptTemplate)
                .WithMany(pt => pt.SurveySteps)
                .HasForeignKey(e => e.AiPromptTemplateId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasIndex(e => e.AiPromptTemplateId);
        }
    }
}
