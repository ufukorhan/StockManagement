using StockManagement.Domain.Entities.Common;

namespace StockManagement.Domain.Entities;

public sealed class Product : BaseSoftDeletableEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ICollection<ProductPriceHistory> ProductPrices { get; set; }
}
