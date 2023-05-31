namespace WebApi.DTOs;

public class JWTTokenDTO : BaseDTO
{
    public string token { get; set; }
    public DateTime Expires { get; set; }
}