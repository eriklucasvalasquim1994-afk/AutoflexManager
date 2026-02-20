
using Autoflex.Domain.Entities;
namespace Autoflex.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllWithRecipesAsync();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task AddRecipeAsync(ProductRecipe recipe);
    void Update(Product product);
    void Delete(Product product);
}