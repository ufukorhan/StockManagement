namespace StockManagement.Domain.Entities.Common;

public abstract class BaseSoftDeletableEntity :BaseEntity, ISoftDeleteEntity
{
    public DateTime DeletedDate { get; set; }
    public bool IsDeleted { get; set; }
}