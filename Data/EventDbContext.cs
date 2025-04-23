using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Models.Entities;

namespace EventManagementSystem.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}