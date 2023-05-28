namespace WebApi.DTOs;

public class AuthDTO
{

}

public class ResponseRequestTokenDTO
{
    public string AccessToken { get; set; }
    public UserDTO User { get; set; }
}