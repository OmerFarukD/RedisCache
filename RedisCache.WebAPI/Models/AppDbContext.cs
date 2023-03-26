using Microsoft.EntityFrameworkCore;

namespace RedisCache.WebAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new List<Product>()
            {
                new Product{Id = 1,Name = "Kalem",Price = 20.00},
                new Product{Id = 2,Name = "Silgi",Price = 10.00},
                new Product{Id = 3,Name = "Defter",Price = 60.00},
            }
            );
    }
}