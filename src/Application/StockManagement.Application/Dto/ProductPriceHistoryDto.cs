using StockManagement.Domain.Entities;

namespace StockManagement.Application.Dto;

public sealed record ProductPriceHistoryDto(Guid Id, decimal Price,DateTime CreatedDate)
{
    public static IEnumerable<ProductPriceHistoryDto> MapListFrom(IEnumerable<ProductPriceHistory> productPrices)
        => productPrices?.Select(x => new ProductPriceHistoryDto(x.Id, x.Price,x.CreatedDate));
}