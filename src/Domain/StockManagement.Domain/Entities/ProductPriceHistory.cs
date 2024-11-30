using StockManagement.Domain.Entities.Common;

namespace StockManagement.Domain.Entities;

public sealed class ProductPriceHistory :BaseEntity
{
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
