using Webhook.API.Model;
using Webhook.API.Repository;
using Webhook.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddSingleton<InMemoryFineRepository>();
builder.Services.AddSingleton<InMemoryWebhookSubscriptionRepository>();

builder.Services.AddHttpClient<WebhookDispatcher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("webhooks/subscriptions", (CreatWebhookRequest request,
    InMemoryWebhookSubscriptionRepository subscriptionRepository) =>
{
    var subscription = new WebhookSubscription(Guid.NewGuid(), request.EventType, request.ClientId, request.WebhookUrl, DateTime.UtcNow);

    subscriptionRepository.Add(subscription);

    return Results.Ok(subscription);
});

app.MapPost("fines", async (CreateFineRequest request,
    InMemoryFineRepository fineRepository,
    WebhookDispatcher webhookDispatcher) =>
{
    var fine = new Fine(Guid.NewGuid(), request.ClientId, request.DriverName, request.Amount, DateTime.UtcNow);

    fineRepository.Add(fine);

    await webhookDispatcher.DispatchAsync(request.ClientId, "fine.created", fine);

    return Results.Ok(fine);
}).WithTags("Fines");


app.MapGet("fines", (InMemoryFineRepository fineRepository) =>
{
    return Results.Ok(fineRepository.GetAll());
}).WithTags("Fines");

app.Run();
