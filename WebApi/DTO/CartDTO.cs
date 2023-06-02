using FluentValidation;

namespace WebApi.DTOs;

public class CartDTO : BaseDTO
{
    public string UserId { get; set; }
    public List<CartProductDTO> CartProducts { get; set; }
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

public class UpdateCartDTO
{
    public class CartUpdateInstruction
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public string Id { get; set; }
    public List<CartUpdateInstruction> Instruction { get; set; }
}

public class UpdateCartDTOValidator : AbstractValidator<UpdateCartDTO>
{
    public UpdateCartDTOValidator()
    {
        RuleFor(x => x.Instruction).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}