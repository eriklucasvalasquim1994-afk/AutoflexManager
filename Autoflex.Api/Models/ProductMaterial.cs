// Model para a tabela intermediária
using Autoflex.Domain.Entities;

public class ProductMaterial
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int RawMaterialId { get; set; }
    public RawMaterial RawMaterial { get; set; }

    public float QuantityNeeded { get; set; } // Quanto desse material o produto usa
}