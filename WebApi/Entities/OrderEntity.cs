namespace WebApi.Entities;

public class OrderEntity : BaseEntity
{
    public bool DeliveringStatus { get; set; }
    public virtual List<OrderProductEntity> OrderProducts { get; set; }
}