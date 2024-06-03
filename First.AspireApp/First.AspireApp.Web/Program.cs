using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using First.AspireApp.Web;
using First.AspireApp.Web.Components;
using First.AspireApp.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

builder.AddSqlServerDbContext<SupportTicketDbContext>("sqldata");

builder.AddAzureBlobClient("BlobConnectionName");
builder.AddAzureQueueClient("QueueConnectionName");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // In development, create the blob container and queue if they don't exist.
    var blobService = app.Services.GetRequiredService<BlobServiceClient>();
    var docsContainer = blobService.GetBlobContainerClient("fileuploads");

    await docsContainer.CreateIfNotExistsAsync();

    var queueService = app.Services.GetRequiredService<QueueServiceClient>();
    var queueClient = queueService.GetQueueClient("tickets");

    await queueClient.CreateIfNotExistsAsync();

    //using var scope = app.Services.CreateScope();
    //var context = scope.ServiceProvider.GetRequiredService<SupportTicketDbContext>();
    //await context.Database.EnsureCreatedAsync();
    //await context.Database.MigrateAsync(); // Use Migrate() to apply migrations
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
