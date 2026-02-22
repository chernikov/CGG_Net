using CGG.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Resend;

namespace CGG.Application.Services;

public class EmailService : IEmailService
{
    private readonly IResend _resend;
    private readonly ILogger<EmailService> _logger;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(
        IResend resend,
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _resend = resend;
        _logger = logger;
        _fromEmail = configuration["Email:FromEmail"] ?? "noreply@careergg.com";
        _fromName = configuration["Email:FromName"] ?? "Career Guidance Guild";
    }

    public async Task SendWelcomeEmailAsync(
        string toEmail,
        string firstName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var html = GetWelcomeEmailTemplate(firstName);
            
            var message = new EmailMessage();
            message.From = $"{_fromName} <{_fromEmail}>";
            message.To.Add(toEmail);
            message.Subject = "Welcome to Career Guidance Guild!";
            message.HtmlBody = html;

            var response = await _resend.EmailSendAsync(message, cancellationToken);
            
            _logger.LogInformation("Welcome email sent to {Email}, MessageId: {MessageId}", 
                toEmail, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send welcome email to {Email}", toEmail);
            // Don't throw - email failure shouldn't break registration
        }
    }

    public async Task SendVerificationEmailAsync(
        string toEmail, 
        string verificationCode, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var html = GetVerificationEmailTemplate(verificationCode);
            
            var message = new EmailMessage();
            message.From = $"{_fromName} <{_fromEmail}>";
            message.To.Add(toEmail);
            message.Subject = "Verify your email address";
            message.HtmlBody = html;

            var response = await _resend.EmailSendAsync(message, cancellationToken);
            
            _logger.LogInformation("Verification email sent to {Email}, MessageId: {MessageId}", 
                toEmail, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email}", toEmail);
        }
    }

    public async Task SendPasswordResetEmailAsync(
        string toEmail, 
        string resetLink, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var html = GetPasswordResetEmailTemplate(resetLink);
            
            var message = new EmailMessage();
            message.From = $"{_fromName} <{_fromEmail}>";
            message.To.Add(toEmail);
            message.Subject = "Reset your password";
            message.HtmlBody = html;

            var response = await _resend.EmailSendAsync(message, cancellationToken);
            
            _logger.LogInformation("Password reset email sent to {Email}, MessageId: {MessageId}", 
                toEmail, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", toEmail);
        }
    }

    private string GetWelcomeEmailTemplate(string firstName)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
</head>
<body style=""font-family: sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
    <h2 style=""color: #6366f1;"">Вітаємо в Career Guidance Guild!</h2>
    <p>Привіт, <strong>{firstName}</strong>!</p>
    <p>Дякуємо за реєстрацію в нашій системі профорієнтації.</p>
    <p>Ви успішно створили обліковий запис і тепер можете користуватися всіма можливостями платформи.</p>
    
    <div style=""background: #f5f5f5; border-left: 4px solid #6366f1; padding: 15px; margin: 20px 0;"">
        <p style=""margin: 0;""><strong>Що далі?</strong></p>
        <ul style=""margin: 10px 0; padding-left: 20px;"">
            <li>Заповніть свій профіль</li>
            <li>Пройдіть тести на профорієнтацію</li>
            <li>Отримайте персональні рекомендації</li>
        </ul>
    </div>

    <p style=""color: #999; font-size: 12px; margin-top: 40px; border-top: 1px solid #eee; padding-top: 20px;"">
        З повагою,<br/>
        Команда Career Guidance Guild<br/>
        <br/>
        Якщо у вас виникли питання, зв'яжіться з нами.
    </p>
</body>
</html>";
    }

    private string GetVerificationEmailTemplate(string verificationCode)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
</head>
<body style=""font-family: sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
    <h2 style=""color: #6366f1;"">Підтвердження email</h2>
    <p>Дякуємо за реєстрацію!</p>
    <p>Ваш код підтвердження:</p>
    
    <div style=""background: #f5f5f5; border: 2px solid #6366f1; border-radius: 8px; padding: 20px; text-align: center; margin: 30px 0;"">
        <div style=""font-size: 28px; font-weight: bold; color: #6366f1; letter-spacing: 2px; font-family: 'Courier New', monospace;"">
            {verificationCode}
        </div>
    </div>
    
    <p style=""color: #666;"">Код дійсний протягом 24 годин.</p>

    <p style=""color: #999; font-size: 12px; margin-top: 40px; border-top: 1px solid #eee; padding-top: 20px;"">
        Якщо ви не реєструвались на нашому сайті, просто проігноруйте цей email.
    </p>
</body>
</html>";
    }

    private string GetPasswordResetEmailTemplate(string resetLink)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
</head>
<body style=""font-family: sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;"">
    <h2 style=""color: #6366f1;"">Скидання пароля</h2>
    <p>Ви запитали скидання пароля для вашого облікового запису.</p>
    <p>Натисніть кнопку нижче, щоб встановити новий пароль:</p>
    
    <div style=""text-align: center; margin: 30px 0;"">
        <a href=""{resetLink}"" 
           style=""display: inline-block; padding: 12px 24px; background-color: #6366f1; color: white; text-decoration: none; border-radius: 6px; font-weight: bold;"">
            Скинути пароль
        </a>
    </div>

    <p style=""color: #666; font-size: 14px;"">
        Або скопіюйте це посилання у браузер:<br/>
        <a href=""{resetLink}"" style=""color: #6366f1; word-break: break-all;"">{resetLink}</a>
    </p>

    <p style=""color: #999; font-size: 12px; margin-top: 40px; border-top: 1px solid #eee; padding-top: 20px;"">
        Якщо ви не запитували скидання пароля, просто проігноруйте цей email.<br/>
        Посилання дійсне протягом 1 години.
    </p>
</body>
</html>";
    }
}
