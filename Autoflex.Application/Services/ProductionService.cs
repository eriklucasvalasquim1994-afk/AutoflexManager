using Autoflex.Application.DTOs;
using Autoflex.Application.Interfaces;
using Autoflex.Domain.Interfaces;

namespace Autoflex.Application.Services;

public class ProductionService : IProductionService
{
    private readonly IProductRepository _productRepo;
    private readonly IRawMaterialRepository _materialRepo;

    public ProductionService(IProductRepository productRepo, IRawMaterialRepository materialRepo)
    {
        _productRepo = productRepo;
        _materialRepo = materialRepo;
    }

    public async Task<ProductionSuggestionDTO> GetProductionSuggestionAsync()
    {
        // 1. Busca produtos com receitas e ordena pelo maior preço (RF004)
        var products = (await _productRepo.GetAllWithRecipesAsync())
                        .OrderByDescending(p => p.Price);

        // 2. Carrega estoque atual para simulação
        var materials = await _materialRepo.GetAllAsync();

        // Usando 'StockQuantity' conforme seu print da classe RawMaterial
        var virtualStock = materials.ToDictionary(m => m.Id, m => m.StockQuantity);

        var result = new ProductionSuggestionDTO
        {
            SuggestedProducts = new List<ProductResultDTO>()
        };

        foreach (var product in products)
        {
            // Usando 'Recipes' conforme seu print da classe Product
            if (product.Recipes == null || !product.Recipes.Any()) continue;

            int possibleQuantity = int.MaxValue;

            foreach (var item in product.Recipes)
            {
                var available = virtualStock.GetValueOrDefault(item.RawMaterialId);

                // Usando 'RequiredQuantity' conforme seu print da classe ProductRecipe
                if (item.RequiredQuantity > 0)
                {
                    int canMake = (int)(available / item.RequiredQuantity);
                    if (canMake < possibleQuantity) possibleQuantity = canMake;
                }
                else
                {
                    possibleQuantity = 0;
                }
            }

            // 3. Reserva materiais no estoque virtual para o próximo produto da lista
            if (possibleQuantity > 0 && possibleQuantity != int.MaxValue)
            {
                foreach (var item in product.Recipes)
                {
                    virtualStock[item.RawMaterialId] -= (item.RequiredQuantity * possibleQuantity);
                }

                result.SuggestedProducts.Add(new ProductResultDTO
                {
                    ProductName = product.Name,
                    QuantityToProduce = possibleQuantity
                });

                decimal preco = product.Price;
                result.TotalEstimatedValue += preco * possibleQuantity;

            }
        }

        return result;
    }
}