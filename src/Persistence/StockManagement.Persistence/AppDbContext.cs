using Microsoft.EntityFrameworkCore;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Entities.Common;

namespace StockManagement.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPriceHistory> ProductPriceHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable(nameof(Products));
        modelBuilder.Entity<ProductPriceHistory>().ToTable(nameof(ProductPriceHistories));
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditableEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateAuditableEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        UpdateAuditableEntities();
        return base.SaveChanges();
    }

    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State!=EntityState.Detached || e.State!=EntityState.Unchanged);
        foreach (var entityEntry in entries)
        {
            switch (entityEntry.State)
            {
                case EntityState.Deleted when entityEntry.Entity is ISoftDeleteEntity softDeleteEntity:
                    softDeleteEntity.DeletedDate = DateTime.UtcNow;
                    softDeleteEntity.IsDeleted = true;
                    break;
                case EntityState.Modified when entityEntry.Entity is IAuditableEntity auditableEntity:
                    auditableEntity.UpdatedDate =DateTime.UtcNow;
                    break;
                case EntityState.Added when entityEntry.Entity is IAuditableEntity auditableEntity:
                    auditableEntity.CreatedDate =DateTime.UtcNow;
                    break;
            }
        }
    }
}