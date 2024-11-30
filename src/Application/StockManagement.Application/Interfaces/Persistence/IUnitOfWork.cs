namespace StockManagement.Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    Task SaveChangesAsync();
}