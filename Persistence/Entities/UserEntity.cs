using Microsoft.AspNetCore.Identity;

namespace Persistence.Entities;

public class UserEntity : IdentityUser<string>, ITimestamp, ISoftDeletable
{
    public string FullName { get; set; }
    public string Address { get; set; }
    public virtual CartEntity Cart { get; set; }
    public virtual List<OrderEntity> Orders { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public UserEntity() : base()
    {
        ModifiedAt = DateTime.UtcNow;
        CreatedAt = CreatedAt ?? ModifiedAt;
    }
}