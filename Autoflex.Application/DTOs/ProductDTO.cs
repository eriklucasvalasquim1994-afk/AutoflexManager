namespace Autoflex.Application.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<RecipeItemDTO> Recipes { get; set; } = new();
}

public class RecipeItemDTO
{
    public int RawMaterialId { get; set; }
    public string RawMaterialName { get; set; } = string.Empty;
    public double RequiredQuantity { get; set; }
}