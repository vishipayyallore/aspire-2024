var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SupportTickets>("supporttickets");

builder.Build().Run();
