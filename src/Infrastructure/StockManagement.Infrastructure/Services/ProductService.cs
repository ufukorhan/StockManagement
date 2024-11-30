using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StockManagement.Application.Dto;
using StockManagement.Application.Dto.Request;
using StockManagement.Application.Helper;
using StockManagement.Application.Interfaces.Infrastructure;
using StockManagement.Application.Interfaces.Persistence;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Services;

public class ProductService(
    IProductRepository productRepository, 
    IProductPriceHistoryRepository productPriceHistoryRepository,
    IUnitOfWork unitOfWork,
    IOptions<PriceHelper> options) : IProductService
{
    private readonly PriceHelper _priceHelper = options.Value;
    
    public async Task<ProductDto> AddAsync(ProductCreateDto request)
    {
        ValidateAndThrow();
        var result = await productRepository.AddAsync(new Product
        {
           Name = request.Name,
           Price = request.Price
        });
        await unitOfWork.SaveChangesAsync();
        return ProductDto.MapFrom(result);
        void ValidateAndThrow()
        {
            if (request.Price<=decimal.Zero)
                throw new Exception("Fiyat sıfır veya daha küçük olamaz");
        }
    }

    public async Task<ProductDto> UpdateAsync(ProductUpdateDto request)
    {
        ValidateAndThrow();
        var product = await productRepository.GetByIdAsync(request.Id);
        ArgumentNullException.ThrowIfNull(product);
        product.Name = request.Name;

        if (product.Price!=request.Price)
        {
            BackgroundJob.Schedule<IProductService>(service =>
                service.UpdateJobPrice(request.Id,request.Price), _priceHelper.WaitTime.ToTimeSpan());
        }
        
        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync();
        return ProductDto.MapFrom(product);
        void ValidateAndThrow()
        {
            if (request.Price<=decimal.Zero)
                throw new Exception("Fiyat sıfır veya daha küçük olamaz");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await productRepository.GetByIdAsync(id);
        ArgumentNullException.ThrowIfNull(product);
        productRepository.Delete(product);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var product = await productRepository.GetByIdAsync(id);
        ArgumentNullException.ThrowIfNull(product);
        return ProductDto.MapFrom(product);
    }

    public async Task<IEnumerable<ProductDto>> GetListAsync(ProductFilterDto filter)
    {
        var products = await productRepository.GetListAsync(FilterQuery(productRepository.GetAllQuery()));
        return products.Select(ProductDto.MapFrom).ToList();
        IQueryable<Product> FilterQuery(IQueryable<Product> queryable)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
                queryable = queryable.Where(x => x.Name.Contains(filter.Name));
            if (filter.PriceRange != null)
                queryable = queryable.Where(x => x.Price >= filter.PriceRange.Min &&
                                                 x.Price <= filter.PriceRange.Max);
            return queryable;
        }
    }
    
    public async Task UpdateJobPrice(Guid id, decimal newPrice)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product is null || newPrice<=decimal.Zero)
            return;

        var oldPrice = product.Price;
        product.Price = newPrice;
        productRepository.Update(product);
        await productPriceHistoryRepository.AddAsync(new ProductPriceHistory
        {
            ProductId = product.Id,
            Price = oldPrice
        });
        await unitOfWork.SaveChangesAsync();
    }
}