using Microsoft.EntityFrameworkCore;
using Webhook.API.Data;
using Webhook.API.Model;

namespace Webhook.API.Repository;

internal sealed class WebhookSubscriptionRepository(WebhooksDbContext dbContext)
{
    public void Add(WebhookSubscription subscription)
    {
        dbContext.WebhookSubscriptions.Add(subscription);
        dbContext.SaveChanges();
    }

    public async Task<List<WebhookSubscription>> GetbyClientEventType(string clientId, string eventType)
    {
        return await dbContext.WebhookSubscriptions
            .Where(s => s.ClientId == clientId && s.EventType == eventType)
            .AsNoTracking()
            .ToListAsync();
    }
}