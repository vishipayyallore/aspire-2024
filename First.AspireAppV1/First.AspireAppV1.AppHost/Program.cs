var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
//var blobs = builder.AddAzureStorage("storageb")
//                   .AddBlobs("BlobConnection");
//var queues = builder.AddAzureStorage("storageq")
//                    .AddQueues("QueueConnection");

var apiService = builder.AddProject<Projects.First_AspireAppV1_ApiService>("apiservice");

builder.AddProject<Projects.First_AspireAppV1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
