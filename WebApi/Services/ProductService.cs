using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs.ProductDTO;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Services;

public interface IProductService
{
    Task<ProductPaginationResponseDTO> SearchAsync(ProductPaginationRequestDTO paginationRequest);
    Task<ProductDetailDTO> GetProductDetailAsync(string id);
    Task<ProductDetailDTO> CreateAsync(ProductCreateDTO productCreate);
    Task<ProductDetailDTO> UpdateProductAsync(string id, ProductUpdateDTO productUpdate);
    Task<ProductDetailDTO> DeleteProductAsync(string id);
}

public class ProductService : BaseService, IProductService
{
    public ProductService(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, dbContext, mapper)
    {
    }

    public async Task<ProductPaginationResponseDTO> SearchAsync(ProductPaginationRequestDTO paginationRequest)
    {
        var products = _dbContext.Products
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

        products.OrderByDescending(x => x.ModifiedAt);

        var res = _mapper.Map<List<ProductDetailDTO>>(await products.ToListAsync());
        return new ProductPaginationResponseDTO(paginationRequest, res);
    }

    public async Task<ProductDetailDTO> CreateAsync(ProductCreateDTO productCreate)
    {
        var product = await _dbContext.Products
                            .Where(x => productCreate.Name == x.Name)
                            .FirstOrDefaultAsync();

        if (product != null)
        {
            throw new AppException(HttpStatusCode.Conflict, string.Format(ErrorMessages.CONFLICTED_ERROR, "Product", "Name", productCreate.Name));
        }

        var categories = _dbContext.Categories.Where(x => productCreate.CategoryIds.Contains(x.Id));
        if (categories.Count() != productCreate.CategoryIds.Count)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Category", "Id", nameof(productCreate.CategoryIds)));
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

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDetailDTO>(product);
    }

    public async Task<ProductDetailDTO> GetProductDetailAsync(string id)
    {
        var product = await _dbContext.Products
                            .Include(x => x.Categories)
                            .ThenInclude(x => x.Category)
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "Id", id));
        }

        return _mapper.Map<ProductDetailDTO>(product);
    }

    public async Task<ProductDetailDTO> UpdateProductAsync(string Id, ProductUpdateDTO productUpdate)
    {
        var product = await _dbContext.Products
                            .Where(x => Id == x.Id)
                            .FirstOrDefaultAsync();

        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Product", "Id", Id));
        }

        var namedProduct = await _dbContext.Products
                            .Where(x => productUpdate.Name == x.Name && x.Id != Id)
                            .FirstOrDefaultAsync();

        if (namedProduct != null)
        {
            throw new AppException(HttpStatusCode.Conflict, string.Format(ErrorMessages.CONFLICTED_ERROR, "Product", "Name", productUpdate.Name));
        }

        var categories = _dbContext.Categories.Where(x => productUpdate.CategoryIds.Contains(x.Id));
        if (categories.Count() != productUpdate.CategoryIds.Count)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "Category", "Id", nameof(productUpdate.CategoryIds)));
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
        product.Price = productUpdate.Price;
        product.Name = productUpdate.Name;
        product.Quantity = productUpdate.Quantity;
        product.PublicStatus = productUpdate.PublicStatus;
        product.ImgSrc = productUpdate.ImgSrc;
        product.Categories = productCategories;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDetailDTO>(product);
    }
    public async Task<ProductDetailDTO> DeleteProductAsync(string id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            throw new AppException(HttpStatusCode.NotFound, string.Format(ErrorMessages.NOT_FOUND_ERROR, "product", "id", id));
        }

        _dbContext.Products.Remove(product);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDetailDTO>(product);
    }
}