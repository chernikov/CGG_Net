using System.Text.Json.Serialization;

namespace CGG.Application.DTOs.Credits;

/// <summary>
/// Webhook payload sent by Monobank when a payment status changes.
/// </summary>
public class MonobankWebhookPayload
{
    [JsonPropertyName("invoiceId")]
    public string? InvoiceId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>Amount in coins (kopecks). Divide by 100 to get UAH.</summary>
    [JsonPropertyName("amount")]
    public long? Amount { get; set; }

    /// <summary>Currency code. 980 = UAH.</summary>
    [JsonPropertyName("ccy")]
    public int? Ccy { get; set; }

    [JsonPropertyName("merchantPaymInfo")]
    public MonobankMerchantPaymInfo? MerchantPaymInfo { get; set; }
}

public class MonobankMerchantPaymInfo
{
    /// <summary>Our order reference (OrderId stored in Transaction).</summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}
