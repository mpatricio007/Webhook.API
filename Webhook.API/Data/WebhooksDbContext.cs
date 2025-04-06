using Microsoft.EntityFrameworkCore;
using Webhook.API.Model;

namespace Webhook.API.Data;

internal sealed class WebhooksDbContext(DbContextOptions<WebhooksDbContext> options) :DbContext(options)
{
    public DbSet<Fine> Fines { get; set; }
    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
    public DbSet<WebhookDeliveryAttempt> WebhookDeliveryAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fine>(builder =>
        {
            builder.ToTable("fines");
            builder.HasKey(f => f.Id);
        });

        modelBuilder.Entity<WebhookSubscription>(builder =>
        {
            builder.ToTable("subscriptions");
            builder.HasKey(s => s.Id);
        });

        modelBuilder.Entity<WebhookDeliveryAttempt>(builder =>
        {
            builder.ToTable("delivery_attempts");
            builder.HasKey(s => s.Id);

            builder.HasOne<WebhookSubscription>()
            .WithMany()
            .HasForeignKey(W => W.WebhookSubscriptionId);
        });
    }
}
