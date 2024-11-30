using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StockManagement.Application.Helper;
using StockManagement.Application.Interfaces.Persistence;
using StockManagement.Domain.Entities;

namespace StockManagement.Persistence;

public sealed class ProductRepository(AppDbContext context,
    IMemoryCache memoryCache,
    IOptions<CacheHelper> options) : Repository<Product>(context), IProductRepository
{
    private readonly CacheHelper _cacheHelper = options.Value;
    public override IQueryable<Product> GetAllQuery()
    {
        return base.GetAllQuery().Include(x => x.ProductPrices);
    }

    public override async Task<Product> GetAsync(IQueryable<Product> queryable)
    {
        queryable = queryable.Include(x => x.ProductPrices);
        var cacheKey = queryable.ToQueryString();
        return await memoryCache.GetOrCreate(
            cacheKey,
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = _cacheHelper.DefaultCacheTime.ToTimeSpan();
                return await base.GetAsync(queryable);
            });
    }

    public override async Task<IReadOnlyCollection<Product>> GetListAsync(IQueryable<Product> queryable)
    {
        queryable = queryable.Include(x => x.ProductPrices);
        var cacheKey = queryable.ToQueryString();
        return await memoryCache.GetOrCreateAsync(
            cacheKey,
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = _cacheHelper.DefaultCacheTime.ToTimeSpan();
                return await base.GetListAsync(queryable);
            });
    }

    public override async Task<Product> GetByIdAsync(Guid id)
    {
        var query = DbSet.Include(x => x.ProductPrices);
        var cacheKey = DbSet.Include(x => x.ProductPrices).ToQueryString();
        return await memoryCache.GetOrCreateAsync(
            cacheKey, async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = _cacheHelper.DefaultCacheTime.ToTimeSpan();
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            });
    }
}