using WebApi.DTOs.OrderProductDTO;
using WebApi.DTOs.UserDTO;

namespace WebApi.DTOs.OrderDTO;

public class OrderDetailDTO : BaseDTO
{
    public UserDetailDTO User { get; set; }
    public string Status { get; set; }
    public List<OrderProductDetailDTO> Products { get; set; }
}