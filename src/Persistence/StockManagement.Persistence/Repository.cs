using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Interfaces.Persistence;
using StockManagement.Domain.Entities.Common;

namespace StockManagement.Persistence;

public abstract class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> DbSet = context.Set<T>();
    public async Task<T> AddAsync(T entity) => (await DbSet.AddAsync(entity)).Entity;
    public void Update(T entity) => DbSet.Update(entity);
    public void Delete(T entity) => DbSet.Remove(entity);
    public virtual IQueryable<T> GetAllQuery() => DbSet.AsNoTracking();
    public virtual async Task<T> GetAsync(IQueryable<T> queryable) => await queryable.FirstOrDefaultAsync();
    public virtual async Task<IReadOnlyCollection<T>> GetListAsync(IQueryable<T> queryable) => await queryable.AsNoTracking().ToListAsync();
    public virtual async Task<T> GetByIdAsync(Guid id) => await DbSet.FirstOrDefaultAsync(x => x.Id == id);
}