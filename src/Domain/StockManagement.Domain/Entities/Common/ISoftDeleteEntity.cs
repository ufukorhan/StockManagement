namespace StockManagement.Domain.Entities.Common;

public interface ISoftDeleteEntity
{
    public DateTime DeletedDate { get; set; }
    public bool IsDeleted { get; set; }
}