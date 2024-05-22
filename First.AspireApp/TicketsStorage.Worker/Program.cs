using TicketsStorage.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddAzureQueueClient("QueueConnectionName");

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
