using FluentValidation;

namespace WebApi.DTOs;

public class CartDTO : BaseDTO
{
    public string UserId { get; set; }
    public List<CartProductDTO> Products { get; set; }
}

public class UserCartDTO
{
    public string UserId { get; set; }
}

public class UserCartDTOValidator : AbstractValidator<UserCartDTO>
{
    public UserCartDTOValidator()
    {
        RuleFor(x => x.UserId).Length(36).WithMessage("Invalid UserId");
    }
}