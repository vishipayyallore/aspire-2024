var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SupportTickets>("supporttickets");

builder.AddProject<Projects.SupportTickets_Worker>("supporttickets.worker");

builder.Build().Run();
