using StockManagement.Domain.Entities;

namespace StockManagement.Application.Dto;

public sealed record ProductDto(Guid Id,
    string Name,
    decimal Price,
    DateTime CreatedDate,
    DateTime UpdatedDate,
    IEnumerable<ProductPriceHistoryDto> ProductPrices=null)
{
    public static ProductDto MapFrom(Product product)
        => new(product.Id,
            product.Name,
            product.Price,
            product.CreatedDate,
            product.UpdatedDate,
            ProductPriceHistoryDto.MapListFrom(product.ProductPrices));
}