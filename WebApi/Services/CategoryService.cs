using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;

namespace WebApi.Services;

public interface ICategoryService
{
    Task<List<CategoryDTO>> Get();
    Task<CategoryDTO> Get(string id);
    Task Create(CreateCategoryDTO categoryCreate);
    Task<CategoryDTO> Update(string id, UpdateCategoryDTO updateCart);
    Task<CategoryDTO> Delete(string id);
}

public class CategoryService : BaseService<CategoryEntity>, ICategoryService
{
    private readonly DbSet<ProductEntity> _productDbSet;
    private readonly DbSet<CartProductEntity> _categoryProductDbSet;
    public CategoryService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
        _productDbSet = dbContext.Products;
        _categoryProductDbSet = dbContext.CartProducts;
    }

    public async Task Create(CreateCategoryDTO categoryCreate)
    {
        var category = await _dbSet.Where(x => x.Name == categoryCreate.Name).FirstOrDefaultAsync();

        if (category != null)
        {
            throw new AppException(HttpStatusCode.Conflict, String.Format(ErrorMessages.CONFLICTED_ERROR, "Category", "name", categoryCreate.Name));
        }

        category = new CategoryEntity()
        {
            Name = categoryCreate.Name,
        };

        await _dbSet.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CategoryDTO> Get(string id)
    {
        var category = await _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "category", "Id", id));
        }

        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<List<CategoryDTO>> Get()
    {
        return _mapper.Map<List<CategoryDTO>>(await _dbSet.ToListAsync());
    }

    public async Task<CategoryDTO> Update(string id, UpdateCategoryDTO updateCategory)
    {
        var category = await _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "category", "Id", id));
        }

        var existedCategory = await _dbSet.Where(x => x.Name == updateCategory.Name && x.Id != id).FirstOrDefaultAsync();
        if (existedCategory != null)
        {
            throw new AppException(HttpStatusCode.Conflict, String.Format(ErrorMessages.CONFLICTED_ERROR, "Category", "name", updateCategory.Name));
        }

        category.Name = updateCategory.Name;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<CategoryDTO> Delete(string id)
    {
        var category = await _dbSet.FindAsync(id);
        _dbSet.Remove(category);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CategoryDTO>(category);
    }
}
