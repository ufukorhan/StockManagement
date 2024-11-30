namespace StockManagement.Domain.Entities.Common;

public interface IAuditableEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}