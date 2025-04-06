using Webhook.API.Data;
using Webhook.API.Model;

namespace Webhook.API.Repository;

internal sealed class WebhookDeliveryAttemptRepository(WebhooksDbContext dbContext)
{
    public void Add(WebhookDeliveryAttempt deliveryAttempt)
    {
        dbContext.WebhookDeliveryAttempts.Add(deliveryAttempt);
        dbContext.SaveChanges();
    }
}