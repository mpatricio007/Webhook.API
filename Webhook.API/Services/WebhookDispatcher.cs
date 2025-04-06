using System.Text.Json;
using Webhook.API.Model;
using Webhook.API.Repository;

namespace Webhook.API.Services;

internal sealed class WebhookDispatcher(IHttpClientFactory httpClientFactory, 
    WebhookSubscriptionRepository subscriptionRepository,
    WebhookDeliveryAttemptRepository webhookDeliveryAttemptRepository)
{
    public async Task DispatchAsync<T>(string clientId, string eventType, T data)
    {
        var subscriptions = await subscriptionRepository.GetbyClientEventType(clientId, eventType);

        using var httpClient = httpClientFactory.CreateClient();

        foreach (WebhookSubscription webhookSubscription in subscriptions)
        {
            var payload = new WebhookPayload<T>
            {
                Id = Guid.NewGuid(),
                EventType = webhookSubscription.EventType,
                SubscriptionId = webhookSubscription.Id,
                TimeStamp = DateTime.UtcNow,
                Data = data
            };

            var jsonPayload = JsonSerializer.Serialize(payload);

            try
            {
                var response = await httpClient.PostAsJsonAsync(webhookSubscription.WebhookUrl, payload);
                var attempt = new WebhookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    WebhookSubscriptionId = webhookSubscription.Id,
                    Payload = jsonPayload,
                    ResponseStatusCode = (int)response.StatusCode,
                    Success = response.IsSuccessStatusCode,
                    Timestamp = DateTime.UtcNow
                };

                webhookDeliveryAttemptRepository.Add(attempt);
            }
            catch (Exception e)
            {

                var attempt = new WebhookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    WebhookSubscriptionId = webhookSubscription.Id,
                    Payload = jsonPayload,
                    ResponseStatusCode = null,
                    Success = false,
                    Timestamp = DateTime.UtcNow,
                    ExceptionDetail = e.Message
                };

                webhookDeliveryAttemptRepository.Add(attempt);
            }
            
        }
    }
}
