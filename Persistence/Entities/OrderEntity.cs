namespace Persistence.Entities;

public class OrderEntity : BaseEntity
{
    public UserEntity User { get; set; }
    public string UserId { get; set; }
    public bool DeliveringStatus { get; set; }
    public virtual List<OrderProductEntity> OrderProducts { get; set; }
}