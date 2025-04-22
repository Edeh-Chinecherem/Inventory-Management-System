using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Sale> Sales { get; set; } // Add Sales DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the relationship between Product and Category
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        // Configure the relationship between Sale and Product
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Product)
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        base.OnModelCreating(modelBuilder);
    }
}