using Microsoft.EntityFrameworkCore;
using RedisExampleApi.Models;

namespace RedisExampleApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product {Id=1,Name="TestProduct1",Price=10 },
                new Product {Id=2,Name="TestProduct2",Price=11 },
                new Product {Id=3,Name="TestProduct3",Price=12 },
                new Product {Id=4,Name="TestProduct4",Price=13 },
                new Product {Id=5,Name="TestProduct5",Price=14 },
                new Product {Id=6,Name="TestProduct6",Price=15 },
                new Product {Id=7,Name="TestProduct7",Price=16 },
                new Product {Id=8,Name="TestProduct8",Price= 17 }
        );

            base.OnModelCreating(modelBuilder);
        }
    }
}
