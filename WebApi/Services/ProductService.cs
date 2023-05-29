using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using WebApi.DTOs;
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

}