namespace CGG.Application.Settings;

public class MonobankSettings
{
    public required string Token { get; set; }
    public required string WebhookUrl { get; set; }
    public string ReturnUrl { get; set; } = "/payment/result";
    public string BaseUrl { get; set; } = "https://api.monobank.ua";
    public int ValiditySeconds { get; set; } = 86400;
}
