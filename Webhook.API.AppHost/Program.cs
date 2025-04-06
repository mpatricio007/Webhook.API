var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("webhooks");
    

builder.AddProject<Projects.Webhook_API>("webhook-api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
