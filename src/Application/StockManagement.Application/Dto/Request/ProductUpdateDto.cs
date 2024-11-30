namespace StockManagement.Application.Dto.Request;

public sealed class ProductUpdateDto : ProductCreateDto
{
    public Guid Id { get; set; }
}