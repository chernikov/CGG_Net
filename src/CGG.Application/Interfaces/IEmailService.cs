namespace CGG.Application.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string displayName, CancellationToken cancellationToken = default);
    Task SendVerificationEmailAsync(string toEmail, string verificationCode, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default);
}
