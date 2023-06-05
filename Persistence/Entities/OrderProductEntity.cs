namespace Persistence.Entities;

public class OrderProductEntity : BaseEntity
{
    public int Quantity { get; set; }
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public virtual OrderEntity Order { get; set; }
    public virtual ProductEntity Product { get; set; }
}