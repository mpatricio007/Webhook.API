using Microsoft.EntityFrameworkCore;
using Webhook.API.Data;
using Webhook.API.Model;

namespace Webhook.API.Repository;

internal sealed class FineRepository(WebhooksDbContext dbContext)
{

    public void Add(Fine fine)
    {
        dbContext.Fines.Add(fine);
        dbContext.SaveChanges();
    }

    public async Task<List<Fine>> GetAll()
    {
        return await dbContext.Fines.ToListAsync();
    }
}
