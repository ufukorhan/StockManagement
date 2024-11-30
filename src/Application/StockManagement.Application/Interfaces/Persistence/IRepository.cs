using StockManagement.Domain.Entities.Common;

namespace StockManagement.Application.Interfaces.Persistence;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T> AddAsync(T entity);
    public void Update(T entity);
    public void Delete(T entity);
    public IQueryable<T> GetAllQuery();
    public Task<T> GetAsync(IQueryable<T> queryable);
    public Task<IReadOnlyCollection<T>> GetListAsync(IQueryable<T> queryable);
    public Task<T> GetByIdAsync(Guid id);
}