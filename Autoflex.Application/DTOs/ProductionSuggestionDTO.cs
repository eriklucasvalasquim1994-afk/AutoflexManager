namespace Autoflex.Application.DTOs;

public class ProductionSuggestionDTO
{
    public List<ProductResultDTO> SuggestedProducts { get; set; } = new();
    public decimal TotalEstimatedValue { get; set; }
}

public class ProductResultDTO
{
    public string ProductName { get; set; } = string.Empty;
    public int QuantityToProduce { get; set; }
}