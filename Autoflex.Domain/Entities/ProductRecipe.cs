using System.Text.Json.Serialization;

namespace Autoflex.Domain.Entities;

public class ProductRecipe
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }

    public int RawMaterialId { get; set; }

    [JsonIgnore]
    public RawMaterial? RawMaterial { get; set; }

    public double RequiredQuantity { get; set; }
}