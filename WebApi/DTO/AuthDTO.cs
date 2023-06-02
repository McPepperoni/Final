using FluentValidation;

namespace WebApi.DTOs;

public class AuthDTO
{

}

public class ResponseRequestTokenDTO
{
    public string AccessToken { get; set; }
    public UserDTO User { get; set; }
}

public class AuthLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class AuthLoginDTOValidator : AbstractValidator<AuthLoginDTO>
{
    public AuthLoginDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
    }
}

public class AuthRefreshTokenDTO
{
    public string RefreshToken { get; set; }
}

public class AuthRefreshTokenDTOValidator : AbstractValidator<AuthRefreshTokenDTO>
{
    public AuthRefreshTokenDTOValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}