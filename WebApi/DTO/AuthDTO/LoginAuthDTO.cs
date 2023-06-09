using FluentValidation;

namespace WebApi.DTOs.AuthDTO;

public class LoginAuthDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class AuthLoginDTOValidator : AbstractValidator<LoginAuthDTO>
{
    public AuthLoginDTOValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
    }
}