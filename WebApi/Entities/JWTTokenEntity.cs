namespace WebApi.Entities;

public class JWTTokenEntity : BaseEntity
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}