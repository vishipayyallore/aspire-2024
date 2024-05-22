var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

//var storage = builder.AddAzureStorage("Storage");

var apiService = builder.AddProject<Projects.First_AspireApp_ApiService>("apiservice");

builder.AddProject<Projects.First_AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.TicketsStorage_Worker>("ticketsstorage-worker");

builder.Build().Run();
