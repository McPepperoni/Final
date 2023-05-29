namespace Persistence.Entities;

public class CartEntity : BaseEntity
{
    public virtual List<CartProductEntity> CartProducts { get; set; }
}