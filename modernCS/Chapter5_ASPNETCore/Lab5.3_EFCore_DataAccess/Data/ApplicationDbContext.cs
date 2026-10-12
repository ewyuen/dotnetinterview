using EFCoreDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial data
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "High-performance laptop for developers",
                Price = 1299.99m,
                StockQuantity = 10,
                CreatedAt = new DateTime(2025, 1, 1)
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Description = "Wireless ergonomic mouse",
                Price = 49.99m,
                StockQuantity = 25,
                CreatedAt = new DateTime(2025, 1, 1)
            },
            new Product
            {
                Id = 3,
                Name = "Keyboard",
                Description = "Mechanical keyboard with RGB lighting",
                Price = 149.99m,
                StockQuantity = 15,
                CreatedAt = new DateTime(2025, 1, 1)
            }
        );
    }
}