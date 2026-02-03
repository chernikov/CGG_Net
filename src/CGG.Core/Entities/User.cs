using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CGG.Core.Entities
{
    public enum UserRole
    {
        UserChild,
        UserParent,
        Teacher,
        Admin
    }

    public class User : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserRole Role { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public Guid? MemberId { get; set; }
        public Member? Member { get; set; }
        
        public Guid? SchoolId { get; set; }
        public School? School { get; set; }

        // Collections
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
        public ICollection<AIRecommendation> AIRecommendations { get; set; } = new List<AIRecommendation>();
    }

    public class Family
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class Member
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public Guid FamilyId { get; set; }
        public Family Family { get; set; } = null!;
        
        public string Role { get; set; } = "child";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PaymentId { get; set; }
        public string Status { get; set; } = "completed";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
    }

    public class SurveyResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string SurveyType { get; set; } = string.Empty;
        public string Answers { get; set; } = "{}";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class AIRecommendation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Content { get; set; } = string.Empty;
        public string? Prompt { get; set; }
        public int TokensUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PromoCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Credits { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; } = 0;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
