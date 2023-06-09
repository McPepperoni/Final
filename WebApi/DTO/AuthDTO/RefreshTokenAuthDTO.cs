using FluentValidation;

namespace WebApi.DTOs.AuthDTO;

public class RefreshTokenAuth
{
    public string RefreshToken { get; set; }
}

public class AuthRefreshTokenDTOValidator : AbstractValidator<RefreshTokenAuth>
{
    public AuthRefreshTokenDTOValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}