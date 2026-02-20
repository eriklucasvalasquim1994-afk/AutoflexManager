using Autoflex.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Autoflex.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<RawMaterial> RawMaterials { get; set; }
    public DbSet<ProductRecipe> ProductRecipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<RawMaterial>().HasKey(m => m.Id);

            
        modelBuilder.Entity<ProductRecipe>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.Recipes)
            .HasForeignKey(pr => pr.ProductId);

        modelBuilder.Entity<ProductRecipe>()
            .HasOne(pr => pr.RawMaterial)
            .WithMany()
            .HasForeignKey(pr => pr.RawMaterialId);

        base.OnModelCreating(modelBuilder);
    }
}