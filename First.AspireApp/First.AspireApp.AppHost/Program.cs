var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

// Automatically provision an Application Insights resource
var appinsights = builder.AddAzureApplicationInsights("applicationinsights");

var apiService = builder.AddProject<Projects.First_AspireApp_ApiService>("apiservice")
    .WithReference(appinsights);

builder.AddProject<Projects.First_AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(appinsights)
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.TicketsStorage_Worker>("ticketsstorage-worker")
    .WithReference(appinsights);

builder.AddContainer("prometheus", "prom/prometheus")
       .WithBindMount("../prometheus", "/etc/prometheus", isReadOnly: true)
       .WithHttpEndpoint(/* This port is fixed as it's referenced from the Grafana config */ port: 9090, targetPort: 9090);

builder.Build().Run();
