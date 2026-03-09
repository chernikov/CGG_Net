namespace CGG.Core.Entities
{
    public class WebhookLog
    {
        public Guid Id { get; set; }

        /// <summary>Source system (e.g. "monobank").</summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>Monobank invoiceId or other external identifier.</summary>
        public string? ExternalId { get; set; }

        /// <summary>Payment orderId reference.</summary>
        public string? OrderId { get; set; }

        /// <summary>Status reported by the webhook (e.g. "success", "failure", "pending").</summary>
        public string? Status { get; set; }

        /// <summary>Raw JSON body as received from the external system.</summary>
        public string? RawBody { get; set; }

        /// <summary>Processing result: "ok", "skipped", "error".</summary>
        public string ProcessingResult { get; set; } = "ok";

        /// <summary>Error message if processing failed.</summary>
        public string? Error { get; set; }

        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
