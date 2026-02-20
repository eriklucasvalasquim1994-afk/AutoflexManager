using Autoflex.Domain.Entities;
using Autoflex.Domain.Interfaces;
using Autoflex.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Autoflex.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllWithRecipesAsync()
    {
        return await _context.Products
            .Include(p => p.Recipes)
                .ThenInclude(r => r.RawMaterial)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Recipes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges(); // No Update/Delete simples podemos usar síncrono ou async
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    public async Task AddRecipeAsync(ProductRecipe recipe)
    {
        _context.ProductRecipes.Add(recipe);
        await _context.SaveChangesAsync();
    }
}