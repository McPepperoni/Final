using FluentValidation;

namespace WebApi.DTOs.CartDTO;

public class AddToCartDTO
{
    public string ItemId { get; set; }
    public int Quantity { get; set; }
}

public class AddToCartDTOValidator : AbstractValidator<AddToCartDTO>
{
    public AddToCartDTOValidator()
    {
        RuleFor(x => x.ItemId).Id();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}