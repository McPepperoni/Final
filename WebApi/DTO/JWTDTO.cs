namespace WebApi.DTOs;

public class JWTTokenDTO : BaseDTO
{
    public string token { get; set; }
    public DateTime Expires { get; set; }
}

public class AuthTokenResultDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}