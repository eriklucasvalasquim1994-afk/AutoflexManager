using Autoflex.Domain.Common;

namespace Autoflex.Domain.Entities;

public class RawMaterial: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public double StockQuantity { get; set; }           
}
