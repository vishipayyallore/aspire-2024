using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;

namespace TicketsStorage.Worker;

public class Worker(ILogger<Worker> logger, QueueServiceClient client) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly QueueServiceClient _client = client ?? throw new ArgumentNullException(nameof(client));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.BeginScope("Worker is starting.");

        var queueClient = _client.GetQueueClient("tickets");
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            QueueMessage[] messages =
                await queueClient.ReceiveMessagesAsync(
                    maxMessages: 25, cancellationToken: stoppingToken);

            foreach (var message in messages)
            {
                _logger.LogInformation(
                    "Message from queue: {Message}", message.MessageText);

                await queueClient.DeleteMessageAsync(
                    message.MessageId,
                    message.PopReceipt,
                    cancellationToken: stoppingToken);
            }

            logger.LogInformation("Worker is waiting for more messages.");
            // TODO: Determine an appropriate time to wait 
            // before checking for more messages.
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
