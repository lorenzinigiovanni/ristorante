using RistoranteDigitaleServer.Models;
using Microsoft.EntityFrameworkCore;

namespace RistoranteDigitaleServer.Services
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Items)
                .WithMany(e => e.Orders)
                .UsingEntity<OrderItem>();
        }

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItem { get; set; } = null!;
    }
}
