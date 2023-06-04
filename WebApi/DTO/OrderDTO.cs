using FluentValidation;

namespace WebApi.DTOs;
public class OrderDTO : BaseDTO
{
    public UserDTO User { get; set; }
    public bool DeliveringStatus { get; set; }
    public List<OrderProductDTO> Products { get; set; }
}

public class CreateOrderDTO
{
    public List<CreateOrderProductDTO> Products { get; set; }
}

public class CreateOrderDTOValidator : AbstractValidator<CreateOrderDTO>
{
    public CreateOrderDTOValidator()
    {
    }
}