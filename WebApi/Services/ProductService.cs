using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;

public interface IProductService
{
    Task<PaginationResponseDTO<ProductDTO>> Get(ProductPaginationRequestDTO paginationRequest);
    Task<ProductDTO> Get(string id);
    Task<ProductDTO> Create(ProductCreateDTO productCreate);
    Task<ProductDTO> Update(string id, ProductUpdateDTO productUpdate);
    Task<ProductDTO> Delete(string id);
}

public class ProductService : BaseService<ProductEntity>, IProductService
{
    private readonly DbSet<CategoryEntity> _categoryDbSet;
    public ProductService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
        _categoryDbSet = dbContext.Categories;
    }

    public async Task<PaginationResponseDTO<ProductDTO>> Get(ProductPaginationRequestDTO paginationRequest)
    {
        var products = _dbSet
                        .Where(x => x.Price > paginationRequest.PriceMin && x.Price < paginationRequest.PriceMax);

        if (!string.IsNullOrEmpty(paginationRequest.SearchTerm))
        {
            products = products
                        .Where(x => EF.Functions.Like(x.Name.ToUpper(), $"%{paginationRequest.SearchTerm.ToUpper()}%"));
        }

        if (paginationRequest.PublicStatus)
        {
            products = products.Where(x => x.PublicStatus == paginationRequest.PublicStatusValue);
        }

        products = products
                    .Include(x => x.Categories)
                    .ThenInclude(x => x.Category);

        if (!string.IsNullOrEmpty(paginationRequest.Categories))
        {
            var categories = paginationRequest.Categories.Split(",").ToList();
            products = products.Where(p => p.Categories.Any(x => categories.Contains(x.Category.Name)));
        }

        var res = _mapper.Map<List<ProductDTO>>(await products.ToListAsync());
        return new PaginationResponseDTO<ProductDTO>(paginationRequest, res);
    }


    public async Task<ProductDTO> Create(ProductCreateDTO productCreate)
    {
        var product = await _dbSet
                            .Where(x => productCreate.Name == x.Name)
                            .FirstOrDefaultAsync();

        if (product != null)
        {
            throw new AppException(HttpStatusCode.Conflict, String.Format(ErrorMessages.CONFLICTED_ERROR, "Product", "Name", productCreate.Name));
        }

        var categories = _categoryDbSet.Where(x => productCreate.CategoryIds.Contains(x.Id));
        if (categories.Count() != productCreate.CategoryIds.Count)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Category", "Id", nameof(productCreate.CategoryIds)));
        }

        var productCategories = new List<ProductCategoryEntity>();
        foreach (var item in categories)
        {
            productCategories.Add(new ProductCategoryEntity
            {
                Category = item,
                Product = product,
            });
        }
        product = _mapper.Map<ProductEntity>(productCreate);

        product.Categories = productCategories;

        await _dbSet.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> Get(string id)
    {
        var product = await _dbSet
                            .Include(x => x.Categories)
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "Id", id));
        }

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> Update(string Id, ProductUpdateDTO productUpdate)
    {
        var product = await _dbSet
                            .Where(x => Id == x.Id)
                            .FirstOrDefaultAsync();

        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "Id", Id));
        }

        var namedProduct = await _dbSet
                            .Where(x => productUpdate.Name == x.Name)
                            .FirstOrDefaultAsync();

        if (namedProduct != null)
        {
            throw new AppException(HttpStatusCode.Conflict, String.Format(ErrorMessages.CONFLICTED_ERROR, "Product", "Name", productUpdate.Name));
        }

        var categories = _categoryDbSet.Where(x => productUpdate.CategoryIds.Contains(x.Id));
        if (categories.Count() != productUpdate.CategoryIds.Count)
        {
            throw new AppException(HttpStatusCode.NotFound, String.Format(ErrorMessages.NOT_FOUND_ERROR, "Category", "Id", nameof(productUpdate.CategoryIds)));
        }

        var productCategories = new List<ProductCategoryEntity>();
        foreach (var item in categories)
        {
            productCategories.Add(new ProductCategoryEntity
            {
                Category = item,
                Product = product,
            });
        }

        product.Categories = productCategories;
        product.Name = productUpdate.Name;
        product.Quantity = productUpdate.Quantity;
        product.PublicStatus = productUpdate.PublicStatus;
        product.ImgSrc = productUpdate.ImgSrc;
        product.Categories = productCategories;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(product);
    }
    public async Task<ProductDTO> Delete(string id)
    {
        var product = await _dbSet.FindAsync(id);

        _dbSet.Remove(product);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(product);
    }
}