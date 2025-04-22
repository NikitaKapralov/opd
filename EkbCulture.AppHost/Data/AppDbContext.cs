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
            // Пока оставляем пустым
        }
    }
}