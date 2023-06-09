namespace Persistence.Entities;

public class CartProductEntity : BaseEntity
{
    public int Quantity { get; set; }
    public string CartId { get; set; }
    public string ProductId { get; set; }
    public virtual ProductEntity Product { get; set; }
    public virtual CartEntity Cart { get; set; }
}