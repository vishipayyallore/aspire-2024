using Microsoft.EntityFrameworkCore;
using TicketsStorage.Worker;
using TicketsStorage.Worker.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddAzureQueueClient("QueueConnectionName");

// Add services to the container.
builder.Services.AddDbContext<SupportTicketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqldata")));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
