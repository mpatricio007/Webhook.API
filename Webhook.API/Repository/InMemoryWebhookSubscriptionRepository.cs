using Webhook.API.Model;

namespace Webhook.API.Repository;

internal sealed class InMemoryWebhookSubscriptionRepository
{
    private readonly List<WebhookSubscription> _subscriptions = [];
    public void Add(WebhookSubscription fine)
    {
        _subscriptions.Add(fine);
    }

    public IReadOnlyList<WebhookSubscription> GetbyClientEventType(string clientId, string eventType)
    {
        return _subscriptions.Where(s => s.ClientId == clientId && s.EventType == eventType).ToList().AsReadOnly();
    }
}