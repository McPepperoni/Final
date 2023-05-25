using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs;
public class PaginationRequestDTO
{
    public int Page { get; set; } = 0;
    public int PerPage { get; set; } = 20;
}

public class PaginationResponseDTO<T> where T : class
{
    public int Count { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
    public int PerPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPrevPage { get; set; }
    public List<T> Data { get; set; }

    public PaginationResponseDTO() { }

    public PaginationResponseDTO(PaginationRequestDTO paginationRequest, List<T> data)
    {
        Count = data.Count;
        PerPage = paginationRequest.PerPage;
        PageCount = (int)MathF.Ceiling((float)data.Count / (float)paginationRequest.PerPage);
        if (paginationRequest.Page >= PageCount)
        {
            Page = PageCount - 1;
        }
        else if (paginationRequest.Page < 0)
        {
            Page = 0;
        }
        else
        {
            Page = paginationRequest.Page;
        }

        HasNextPage = Page < PageCount - 1 ? true : false;
        HasPrevPage = Page > 1 ? true : false;

        Data = data.Skip(Page * PerPage).Take(PerPage).ToList();
    }
}

public class ListDTO<T> where T : class
{
    public int Count { get; set; }
    public List<T> Items { get; set; }
}