namespace Persistence.Entities;

public class OrderProductEntity : BaseEntity
{
    public int Quantity { get; set; }
    public virtual ProductEntity Product { get; set; }
}