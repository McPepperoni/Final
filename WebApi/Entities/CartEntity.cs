namespace WebApi.Entities;

public class CartEntity : BaseEntity
{
    public virtual List<CartProductEntity> CartProducts { get; set; }
    public virtual UserEntity User { get; set; }
}