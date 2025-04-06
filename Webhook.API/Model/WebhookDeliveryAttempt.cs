namespace Webhook.API.Model;

public class WebhookDeliveryAttempt
{
    public Guid Id { get; set; }
    public Guid WebhookSubscriptionId { get; set; }

    public string Payload { get; set; }
    public int? ResponseStatusCode { get; set; }

    public bool Success { get; set; }
    public DateTime Timestamp { get; set; }

    public string? ExceptionDetail { get; set; }
}
    
