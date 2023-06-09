namespace WebApi.DTOs.UserDTO;

public class UserDetailDTO : BaseDTO
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}