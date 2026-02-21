namespace CGG.Core.Entities
{
    public class MemberRole
    {
        public Guid MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
