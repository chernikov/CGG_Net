using CGG.Application.DTOs.Auth;
using CGG.Application.Interfaces;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CGG.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<(bool Success, User? User, string? ErrorMessage)> ValidateUserCredentialsAsync(
            string email, 
            string password, 
            CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken);

            if (user == null)
            {
                return (false, null, "Invalid email or password");
            }

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return (false, null, "Invalid email or password");
            }

            // If password needs rehashing (PasswordVerificationResult.SuccessRehashNeeded)
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, password);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return (true, user, null);
        }

        public string GenerateJwtToken(User user, UserTokenContext? context = null)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            context ??= new UserTokenContext { Type = ContextType.System };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("ctx_type", context.Type.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (context.ContextId.HasValue)
                claims.Add(new Claim("ctx_id", context.ContextId.Value.ToString()));

            if (!string.IsNullOrEmpty(context.ContextRole))
                claims.Add(new Claim("ctx_role", context.ContextRole));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<UserTokenContext>> GetAvailableContextsAsync(User user, CancellationToken cancellationToken = default)
        {
            var contexts = new List<UserTokenContext>
            {
                new UserTokenContext { Type = ContextType.System, ContextRole = user.Role.ToString() }
            };

            // Family context via Member
            if (user.FamilyId.HasValue)
            {
                var member = await _unitOfWork.Repository<Member>()
                    .FirstOrDefaultAsync(m => m.UserId == user.Id, cancellationToken);

                if (member != null)
                {
                    var family = await _unitOfWork.Repository<Family>()
                        .GetByIdAsync(member.FamilyId, cancellationToken);

                    var memberRoles = await _unitOfWork.Repository<MemberRole>()
                        .FindAsync(mr => mr.MemberId == member.Id, cancellationToken);

                    foreach (var mr in memberRoles)
                    {
                        var role = await _unitOfWork.Repository<Role>()
                            .GetByIdAsync(mr.RoleId, cancellationToken);

                        contexts.Add(new UserTokenContext
                        {
                            Type = ContextType.Family,
                            ContextId = member.FamilyId,
                            ContextName = family?.Name,
                            ContextRole = role?.Name
                        });
                    }
                }
            }

            // School context
            if (user.SchoolId.HasValue)
            {
                var school = await _unitOfWork.Repository<School>()
                    .GetByIdAsync(user.SchoolId.Value, cancellationToken);

                contexts.Add(new UserTokenContext
                {
                    Type = ContextType.School,
                    ContextId = user.SchoolId,
                    ContextName = school?.Name,
                    ContextRole = user.Role == UserRole.Teacher ? "teacher" : "school-admin"
                });
            }

            return contexts;
        }
    }
}
