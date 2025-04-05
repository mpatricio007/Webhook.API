using Google.Protobuf.WellKnownTypes;
using Webhook.API.Model;
using Webhook.API.Repository;

namespace Webhook.API.Services;

internal sealed class WebhookDispatcher(HttpClient httpClient, InMemoryWebhookSubscriptionRepository subscriptionRepository )
{
    public async Task DispatchAsync(string clientId,string eventType, object payload)
    {
        var subscriptions = subscriptionRepository.GetbyClientEventType(clientId, eventType);
        foreach (WebhookSubscription webhookSubscription in subscriptions) 
        { 
            var request = new
            {
                Id = Guid.NewGuid(),
                webhookSubscription.EventType,
                SubscriptionId = webhookSubscription.Id,
                Timestamp = DateTime.UtcNow,
                Data = payload
            };
            await httpClient.PostAsJsonAsync(webhookSubscription.WebhookUrl, request);
        }

        
    }
}