using Microsoft.AspNetCore.Identity;

namespace Persistence.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string Address { get; set; }
    public virtual CartEntity Cart { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public UserEntity() : base()
    {
        ModifiedAt = DateTime.UtcNow;
        CreatedAt = CreatedAt ?? ModifiedAt;
    }
}