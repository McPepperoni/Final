namespace WebApi.Entities;

public class UserRoleEntity : BaseEntity
{
    public virtual RoleEntity Role { get; set; }
    public virtual UserEntity User { get; set; }
}