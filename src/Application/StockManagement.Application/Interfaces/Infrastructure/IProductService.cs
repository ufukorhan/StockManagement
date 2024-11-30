using StockManagement.Application.Dto;
using StockManagement.Application.Dto.Request;

namespace StockManagement.Application.Interfaces.Infrastructure;

public interface IProductService
{
    Task<ProductDto> AddAsync(ProductCreateDto request);
    Task<ProductDto> UpdateAsync(ProductUpdateDto request);
    Task DeleteAsync(Guid id);
    Task<ProductDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetListAsync(ProductFilterDto filter);
    Task UpdateJobPrice(Guid id, decimal newPrice);
}