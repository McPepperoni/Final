
using Microsoft.EntityFrameworkCore;

namespace WebApi.Entities;

[Index(nameof(UserName), nameof(Email), IsUnique = true)]
public class UserEntity : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public virtual UserInfoEntity UserInfo { get; set; }
}