namespace Persistence.Entities;

public class OrderStatus
{
    public const string DELIVERING = "Delivering";
    public const string WAIT_FOR_APPROVAL = "Wait for approval";
    public const string CANCELED = "Canceled";
    public const string DELIVERED = "Delivered";
}

public class OrderEntity : BaseEntity
{
    public UserEntity User { get; set; }
    public string UserId { get; set; }
    public string Status { get; set; } = OrderStatus.WAIT_FOR_APPROVAL;
    public virtual List<OrderProductEntity> Products { get; set; }
}