using StockManagement.Application.Interfaces.Persistence;
using StockManagement.Domain.Entities;

namespace StockManagement.Persistence;

public sealed class ProductPriceHistoryRepository(AppDbContext context) : Repository<ProductPriceHistory>(context), IProductPriceHistoryRepository;