using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponseDTO<ProductDTO>))]
    public async Task<IActionResult> Get([FromQuery] ProductPaginationRequestDTO paginationRequest)
    => Ok(await _productService.Get(paginationRequest));


}