namespace WebApi.Entities;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; }
    public virtual List<UserRoleEntity> UserRoles { get; set; }
}