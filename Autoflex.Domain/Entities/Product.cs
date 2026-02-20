using Autoflex.Domain.Common;

namespace Autoflex.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public List<ProductRecipe> Recipes { get; set; } = new();
}

