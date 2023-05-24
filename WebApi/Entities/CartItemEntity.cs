namespace WebApi.Entities;

public class CartItemEntity : BaseEntity
{
    public int Quantity { get; set; }
    public virtual ProductEntity Product { get; set; }
}