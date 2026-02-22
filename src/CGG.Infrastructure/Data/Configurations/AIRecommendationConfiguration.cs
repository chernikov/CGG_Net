using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class AIRecommendationConfiguration : IEntityTypeConfiguration<AIRecommendation>
    {
        public void Configure(EntityTypeBuilder<AIRecommendation> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.HasOne(e => e.User)
                .WithMany(u => u.AIRecommendations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.CreatedAt);
        }
    }
}
