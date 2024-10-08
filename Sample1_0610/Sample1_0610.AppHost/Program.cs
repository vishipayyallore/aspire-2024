var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Sample_WebAPI>("sample-webapi");

builder.Build().Run();
