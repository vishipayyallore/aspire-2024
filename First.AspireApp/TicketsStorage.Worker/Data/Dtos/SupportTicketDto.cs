using System.ComponentModel.DataAnnotations;

namespace TicketsStorage.Worker.Data.Dtos;

public class SupportTicketDto
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string? AssignedToName { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
