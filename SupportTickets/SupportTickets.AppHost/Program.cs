var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("Storage");

var blobs = storage.AddBlobs("BlobConnection");
var queues = storage.AddQueues("QueueConnection");

builder.AddProject<Projects.SupportTickets>("supporttickets")
    .WithReference(blobs)
    .WithReference(queues);

builder.AddProject<Projects.SupportTickets_Worker>("supporttickets.worker")
    .WithReference(queues);

builder.Build().Run();
