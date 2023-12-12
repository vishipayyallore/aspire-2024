using SupportTickets.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddAzureQueueService("QueueConnection");

// Add Components before the call to AddServiceDefaults
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
