namespace StockManagement.Application.Dto.Request;

public sealed class PriceRangeDto
{
    public decimal Min { get; set; }
    public decimal Max { get; set; }
}

public sealed class ProductFilterDto
{
    public string Name { get; set; }
    public PriceRangeDto PriceRange { get; set; }
}