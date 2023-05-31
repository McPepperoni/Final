namespace Persistence.Entities;

public class CartProductEntity : BaseEntity
{
    public int Quantity { get; set; }
    public virtual ProductEntity Product { get; set; }
    public virtual CartEntity Cart { get; set; }
}