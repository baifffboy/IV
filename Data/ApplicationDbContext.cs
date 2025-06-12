// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using EcoCollectionService.Models;

namespace EcoCollectionService.Data  // ѕространство имен должно соответствовать
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventSensor> EventSensors { get; set; }
    }
}