using FluentValidation;
using WebApi.DTOs.OrderProductDTO;

namespace WebApi.DTOs.OrderDTO;

public class CreateOrderDTO
{
    public List<CreateOrderProductDTO> Products { get; set; }
}

public class CreateOrderDTOValidator : AbstractValidator<CreateOrderDTO>
{
    public CreateOrderDTOValidator()
    {
        RuleFor(x => x.Products).Must(x => x.Count > 0);
    }
}