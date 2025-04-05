namespace Webhook.API.Model;

public sealed record WebhookSubscription(Guid Id, string EventType, string ClientId, string WebhookUrl,DateTime CreatOnUtc);
public sealed record CreatWebhookRequest(string EventType, string ClientId, string WebhookUrl);
