using CGG.Core.Interfaces;

namespace CGG.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    // Work factor 12 — good balance of security and performance (2^12 iterations)
    private const int WorkFactor = 12;

    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string hashedPassword, string plainPassword)
        => BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
}
