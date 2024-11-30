using Hangfire;
using Hangfire.PostgreSql;
using StockManagement.Application.Helper;
using StockManagement.Infrastructure;
using StockManagement.Persistence;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(appSettings);
    
builder.Services.Configure<PriceHelper>(appSettings.GetSection(nameof(PriceHelper)));
builder.Services.Configure<CacheHelper>(appSettings.GetSection(nameof(CacheHelper)));

builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(x =>
    {
        x.UseNpgsqlConnection(appSettings.GetConnectionString("DefaultConnection"));
    }));

builder.Services.AddHangfireServer();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope()) 
    StockManagement.Persistence.ServiceRegistration.MigrateDatabase(scope.ServiceProvider);
app.UseHangfireDashboard("/dashboard");
app.Run();