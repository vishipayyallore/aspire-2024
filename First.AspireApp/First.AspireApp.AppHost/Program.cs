using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

// Automatically provision an Application Insights resource
var appinsights = builder.AddAzureApplicationInsights("applicationinsights");

var grafana = builder.AddContainer("grafana", "grafana/grafana")
                     .WithBindMount("../grafana/config", "/etc/grafana", isReadOnly: true)
                     .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
                     .WithHttpEndpoint(targetPort: 3000, name: "http");

var apiService = builder.AddProject<Projects.First_AspireApp_ApiService>("apiservice")
    .WithReference(appinsights)
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("http"));

builder.AddProject<Projects.First_AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(appinsights)
    .WithReference(cache)
    .WithReference(apiService)
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("http"));

builder.AddProject<Projects.TicketsStorage_Worker>("ticketsstorage-worker")
    .WithReference(appinsights)
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("http"));

builder.AddContainer("prometheus", "prom/prometheus")
       .WithBindMount("../prometheus", "/etc/prometheus", isReadOnly: true)
       .WithHttpEndpoint(/* This port is fixed as it's referenced from the Grafana config */ port: 9090, targetPort: 9090);

builder.Build().Run();
