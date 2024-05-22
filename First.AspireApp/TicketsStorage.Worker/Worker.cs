using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace TicketsStorage.Worker;

public class Worker(ILogger<Worker> logger, QueueServiceClient client) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly QueueServiceClient _client = client ?? throw new ArgumentNullException(nameof(client));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
