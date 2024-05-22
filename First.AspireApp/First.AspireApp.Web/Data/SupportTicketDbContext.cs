using First.AspireApp.Web.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace First.AspireApp.Web.Data;

public class SupportTicketDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SupportTicketDto> Tickets => Set<SupportTicketDto>();
}