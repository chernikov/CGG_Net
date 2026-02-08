namespace CGG.Core.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public Guid FamilyId { get; set; }
        public Family Family { get; set; } = null!;
        
        public string Role { get; set; } = "child";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Collections
        public ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
        public ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();
    }
}
