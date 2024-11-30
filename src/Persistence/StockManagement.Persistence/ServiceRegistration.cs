using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.Application.Interfaces.Persistence;

namespace StockManagement.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceLayer(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("StockManagement.WebAPI")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductPriceHistoryRepository, ProductPriceHistoryRepository>();
    }
    
    public static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>();

        using var dbContext = new AppDbContext(dbContextOptions);
        dbContext.Database.Migrate();
    }
    
}