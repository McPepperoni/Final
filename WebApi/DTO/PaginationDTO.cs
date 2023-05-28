namespace WebApi.DTOs;
public class PaginationRequestDTO
{
    public int Page { get; set; } = 0;
    public int PerPage { get; set; } = 20;
}

public class PageData
{
    public int Count { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
    public int PerPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPrevPage { get; set; }
}

public class PaginationResponseDTO<TData> where TData : class
{
    public PageData PageData { get; set; }
    public List<TData> Data { get; set; }
    public PaginationRequestDTO Filter { get; set; }

    public PaginationResponseDTO() { }

    public PaginationResponseDTO(PaginationRequestDTO paginationRequest, List<TData> data)
    {
        PageData = new PageData()
        {
            Count = data.Count,
            PerPage = paginationRequest.PerPage,
            PageCount = (int)MathF.Ceiling((float)data.Count / (float)paginationRequest.PerPage)
        };

        Filter = paginationRequest;
        if (paginationRequest.Page >= PageData.PageCount)
        {
            PageData.Page = PageData.PageCount - 1;
        }
        else if (paginationRequest.Page < 0)
        {
            PageData.Page = 0;
        }
        else
        {
            PageData.Page = paginationRequest.Page;
        }

        PageData.HasNextPage = PageData.Page < PageData.PageCount - 1 ? true : false;
        PageData.HasPrevPage = PageData.Page > 0 ? true : false;

        Data = data.Skip(PageData.Page * PageData.PerPage).Take(PageData.PerPage).ToList();
    }
}

public class ListDTO<T> where T : class
{
    public int Count { get; set; }
    public List<T> Items { get; set; }
}