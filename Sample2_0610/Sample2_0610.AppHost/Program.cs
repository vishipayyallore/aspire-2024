var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Sample2_0610_ApiService>("apiservice");

builder.AddProject<Projects.Sample2_0610_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
