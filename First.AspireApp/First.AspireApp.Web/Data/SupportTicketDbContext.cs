using First.AspireApp.Web.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace First.AspireApp.Web.Data;

public class SupportTicketDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SupportTicketDto> Tickets => Set<SupportTicketDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the SupportTicket entity
        modelBuilder.Entity<SupportTicketDto>(entity =>
        {
            // Set the table name
            entity.ToTable("SupportTickets");

            // Set the primary key
            entity.HasKey(e => e.Id);

            // Configure the properties
            entity.Property(e => e.Title)
                .IsRequired() // Mark as required
                .HasMaxLength(100); // Set max length

            entity.Property(e => e.Description)
                .IsRequired() // Mark as required
                .HasMaxLength(1000); // Set max length

            entity.Property(e => e.CreatedAt)
                .IsRequired() // Mark as required
                .HasDefaultValueSql("GETUTCDATE()"); // Set default value
        });

    }

}