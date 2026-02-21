namespace CGG.Core.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
    }
}
