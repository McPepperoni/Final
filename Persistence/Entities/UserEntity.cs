using Microsoft.AspNetCore.Identity;

namespace Persistence.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string Address { get; set; }
}