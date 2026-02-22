using CGG.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CGG.Infrastructure.Data.Configurations
{
    public class MemberRoleConfiguration : IEntityTypeConfiguration<MemberRole>
    {
        public void Configure(EntityTypeBuilder<MemberRole> builder)
        {
            builder.HasKey(e => new { e.MemberId, e.RoleId });

            builder.Property(e => e.AssignedAt)
                .IsRequired();

            builder.HasOne(e => e.Member)
                .WithMany(m => m.MemberRoles)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Role)
                .WithMany(r => r.MemberRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.MemberId);
            builder.HasIndex(e => e.RoleId);
        }
    }
}
