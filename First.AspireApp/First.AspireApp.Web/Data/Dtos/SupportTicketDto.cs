using System.ComponentModel.DataAnnotations;

namespace First.AspireApp.Web.Data.Dtos;

public class SupportTicketDto
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
