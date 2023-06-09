using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.ProductDTO;

namespace WebApi.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductPaginationResponseDTO))]
    public async Task<IActionResult> Get([FromQuery] ProductPaginationRequestDTO paginationRequest)
    => Ok(await _productService.SearchAsync(paginationRequest));

    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDetailDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string id)
    => Ok(await _productService.GetProductDetailAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDetailDTO))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] ProductCreateDTO productCreate)
    => Ok(await _productService.CreateAsync(productCreate));

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDetailDTO))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductUpdateDTO productUpdate)
    => Ok(await _productService.UpdateProductAsync(id, productUpdate));

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDetailDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    => Ok(await _productService.DeleteProductAsync(id));
}