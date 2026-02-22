using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.HasOne(e => e.User)
                .WithOne(u => u.Member)
                .HasForeignKey<Member>(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.HasOne(e => e.Family)
                .WithMany(f => f.Members)
                .HasForeignKey(e => e.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.FamilyId);

            builder.HasMany(e => e.MemberRoles)
                .WithOne(mr => mr.Member)
                .HasForeignKey(mr => mr.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
