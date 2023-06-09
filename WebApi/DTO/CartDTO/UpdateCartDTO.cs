using FluentValidation;

namespace WebApi.DTOs.CartDTO;

public class UpdateCartDTO
{
    public class CartUpdateInstruction
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public List<CartUpdateInstruction> Instruction { get; set; }
}

public class UpdateCartDTOValidator : AbstractValidator<UpdateCartDTO>
{
    public UpdateCartDTOValidator()
    {
        RuleFor(x => x.Instruction).NotEmpty();
    }
}