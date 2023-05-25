namespace WebApi.DTOs;

public class UserRoleDTO : BaseDTO
{
    public UserDTO User { get; set; }
    public RoleDTO Role { get; set; }
}