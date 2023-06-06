namespace Persistence.Entities;

public class CartEntity : BaseEntity, ISoftDeletable
{
    public string UserId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual List<CartProductEntity> CartProducts { get; set; }
}