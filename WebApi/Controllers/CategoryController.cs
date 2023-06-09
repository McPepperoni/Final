using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.CategoryDTO;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDetailDTO>))]
    [HttpGet]
    public async Task<IActionResult> Get()
    => Ok(await _categoryService.Get());

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDetailDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:length(36)}")]
    public async Task<IActionResult> Get(string id)
    => Ok(await _categoryService.Get(id));

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDetailDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id:length(36)}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryDTO updateCategory)
    => Ok(await _categoryService.Update(id, updateCategory));

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDTO updateCategory)
    {
        await _categoryService.Create(updateCategory);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDetailDTO))]
    public async Task<IActionResult> Delete(string id)
     => Ok(await _categoryService.Delete(id));

}