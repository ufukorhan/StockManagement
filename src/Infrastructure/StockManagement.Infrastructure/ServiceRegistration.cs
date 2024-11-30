using Microsoft.Extensions.DependencyInjection;
using StockManagement.Application.Interfaces.Infrastructure;
using StockManagement.Infrastructure.Services;

namespace StockManagement.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
    }
}