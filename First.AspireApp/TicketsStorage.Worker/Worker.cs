using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text;
using System.Text.Json;
using TicketsStorage.Worker.Data;
using TicketsStorage.Worker.Data.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsStorage.Worker;

public class Worker(ILogger<Worker> logger, QueueServiceClient client, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly QueueServiceClient _client = client ?? throw new ArgumentNullException(nameof(client));
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

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

                // Decode the base64-encoded message
                string decodedMessage = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));

                // Deserialize the JSON message into a SupportTicketDto object
                SupportTicketDto supportTicketDto = JsonSerializer.Deserialize<SupportTicketDto>(decodedMessage)!;

                // Now you can use the supportTicket object as needed
                _logger.LogInformation("Received support ticket with title: {Title}", supportTicketDto.Title);

                // Create a new scope
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SupportTicketDbContext>();

                    // Update the support ticket in the database
                    var supportTicket = await dbContext.Tickets.FindAsync(supportTicketDto.Id);
                    if (supportTicket != null)
                    {
                        supportTicket.AssignedToName = $"Name-{Guid.NewGuid():X}";
                        supportTicket.AssignedAt = DateTime.Now;
                        dbContext.Tickets.Update(supportTicket);
                        await dbContext.SaveChangesAsync();
                    }
                }

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
