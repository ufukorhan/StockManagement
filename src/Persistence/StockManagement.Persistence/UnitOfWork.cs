using StockManagement.Application.Interfaces.Persistence;

namespace StockManagement.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private bool _disposed;
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
    public void SaveChanges() => context.SaveChanges();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing) context.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}