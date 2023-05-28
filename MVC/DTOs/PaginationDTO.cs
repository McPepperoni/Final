using System.ComponentModel.DataAnnotations;

namespace MVC.DTOs;
public class PaginationRequestDTO
{
    public int Page { get; set; } = 0;
    public int PerPage { get; set; } = 20;
}

public class PageDataDTO
{
    public int Count { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
    public int PerPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPrevPage { get; set; }
}

public class PaginationResponseDTO<TData, TRequest> where TData : class
{
    public PageDataDTO PageData { get; set; }
    public List<TData> Data { get; set; }
    public TRequest Filter { get; set; }
}

public class ListDTO<T> where T : class
{
    public int Count { get; set; }
    public List<T> Items { get; set; }
}