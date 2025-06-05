using Microsoft.EntityFrameworkCore;
using EkbCulture.AppHost.Models;
namespace EkbCulture.AppHost.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для массивов
            modelBuilder.Entity<Location>()
                .Property(l => l.VisitedBy)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
                );

            modelBuilder.Entity<User>()
                .Property(u => u.VisitedLocations)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
                );
        }
    }
}