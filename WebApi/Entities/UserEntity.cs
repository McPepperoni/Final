
using Microsoft.EntityFrameworkCore;

namespace WebApi.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public virtual UserInfoEntity UserInfo { get; set; }
    public virtual CartEntity Cart { get; set; }
    public virtual List<OrderEntity> Orders { get; set; }
    public virtual List<UserRoleEntity> UserRoles { get; set; }
}