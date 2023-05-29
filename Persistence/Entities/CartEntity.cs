namespace Persistence.Entities;

public class CartEntity : BaseEntity
{
    public string UserId { get; set; }
    public virtual List<CartProductEntity> CartProducts { get; set; }
}