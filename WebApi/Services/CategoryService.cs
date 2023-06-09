using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs.CategoryDTO;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface ICategoryService
{
    Task<List<CategoryDetailDTO>> Get();
    Task<CategoryDetailDTO> Get(string id);
    Task Create(CreateCategoryDTO categoryCreate);
    Task<CategoryDetailDTO> Update(string id, UpdateCategoryDTO updateCart);
    Task<CategoryDetailDTO> Delete(string id);
}

public class CategoryService : BaseService, ICategoryService
{
    private readonly DbSet<ProductEntity> _productDbSet;
    private readonly DbSet<CartProductEntity> _categoryProductDbSet;
    public CategoryService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
        _productDbSet = dbContext.Products;
        _categoryProductDbSet = dbContext.CartProducts;
    }

    public async Task Create(CreateCategoryDTO categoryCreate)
    {
        var category = await _dbContext.Categories.Where(x => x.Name == categoryCreate.Name).FirstOrDefaultAsync();

        if (category != null)
        {
            throw new AppException(HttpStatusCode.Conflict, string.Format(ErrorMessages.CONFLICTED_ERROR, "Category", "name", categoryCreate.Name));
        }

        category = new CategoryEntity()
        {
            Name = categoryCreate.Name,
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CategoryDetailDTO> Get(string id)
    {
        var category = await _dbContext.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "category", "Id", id));
        }

        return _mapper.Map<CategoryDetailDTO>(category);
    }

    public async Task<List<CategoryDetailDTO>> Get()
    {
        return _mapper.Map<List<CategoryDetailDTO>>(await _dbContext.Categories.ToListAsync());
    }

    public async Task<CategoryDetailDTO> Update(string id, UpdateCategoryDTO updateCategory)
    {
        var category = await _dbContext.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "category", "Id", id));
        }

        var existedCategory = await _dbContext.Categories.Where(x => x.Name == updateCategory.Name && x.Id != id).FirstOrDefaultAsync();
        if (existedCategory != null)
        {
            throw new AppException(HttpStatusCode.Conflict, string.Format(ErrorMessages.CONFLICTED_ERROR, "Category", "name", updateCategory.Name));
        }

        category.Name = updateCategory.Name;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CategoryDetailDTO>(category);
    }

    public async Task<CategoryDetailDTO> Delete(string id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        _dbContext.Categories.Remove(category);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CategoryDetailDTO>(category);
    }
}
