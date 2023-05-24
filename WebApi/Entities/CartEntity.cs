namespace WebApi.Entities;

public class CartEntity : BaseEntity
{
    public virtual List<CartItemEntity> CartItems { get; set; }
    public virtual UserEntity User { get; set; }
}