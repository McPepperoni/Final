using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Services;

public interface IProductService
{
    Task<PaginationResponseDTO<ProductDTO>> Get(ProductPaginationRequestDTO paginationRequest);
}

public class ProductService : BaseService<ProductEntity>, IProductService
{
    public ProductService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<PaginationResponseDTO<ProductDTO>> Get(ProductPaginationRequestDTO paginationRequest)
    {
        var products = await _dbSet
                                .Where(x => x.Price > paginationRequest.PriceMin && x.Price < paginationRequest.PriceMax)
                                .Include(x => x.Categories)
                                .ThenInclude(x => x.Category)
                                .ToListAsync();

        if (!string.IsNullOrEmpty(paginationRequest.SearchTerm))
        {
            products = products
                        .Where(x => EF.Functions.Like(x.Name.ToUpper(), $"%{paginationRequest.SearchTerm.ToUpper()}%"))
                        .ToList();
        }

        if (paginationRequest.PublicStatus)
        {
            products = products.Where(x => x.PublicStatus == paginationRequest.PublicStatusValue).ToList();
        }

        if (!string.IsNullOrEmpty(paginationRequest.Categories))
        {
            var categories = paginationRequest.Categories.Split(",").ToList();
            products = products.FindAll(p => p.Categories.Any(x => categories.Contains(x.Category.Name)));
        }

        var res = _mapper.Map<List<ProductDTO>>(products);
        return new PaginationResponseDTO<ProductDTO>(paginationRequest, res);
    }

}