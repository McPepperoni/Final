using WebApi.DTOs.UserDTO;

namespace WebApi.DTOs.AuthDTO;

public class AuthDetailDTO
{
    public string AccessToken { get; set; }
    public UserDetailDTO User { get; set; }
}