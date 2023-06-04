using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace MVC.Areas.Product;

public class QueryString
{
    public string NextPage { get; set; }
    public string PrevPage { get; set; }
}

public class Pagination
{
    public QueryString Query { get; set; }

    public static QueryString GetQueryString(string queryString, int Page)
    { // Get the current query string
        var nextPage = "";
        var prevPage = "";

        if (!queryString.Contains("?"))
        {
            nextPage = "?";
            prevPage = "?";
        }

        if (queryString.Contains("Page="))
        {
            nextPage += Regex.Replace(queryString, @"Page=\d*", $"Page={Page + 1}");
            prevPage += Regex.Replace(queryString, @"Page=\d*", $"Page={Page - 1}");
        }
        else
        {
            nextPage += queryString + $"Page={Page + 2}";
            prevPage += queryString + $"Page={Page}";
        }

        return new()
        {
            NextPage = nextPage,
            PrevPage = prevPage
        };
    }

    public static List<string> GetPagination(int currentPage, int maxPage)
    {
        const int maxVisiblePages = 5; // Number of visible pages (excluding ellipsis)

        List<string> pages = new List<string>();

        if (maxPage <= maxVisiblePages)
        {
            // If the total number of pages is less than or equal to the maximum visible pages,
            // display all pages without ellipsis.
            for (int i = 1; i <= maxPage; i++)
            {
                pages.Add(i.ToString());
            }
        }
        else
        {
            pages.Add("1");

            if (currentPage <= maxVisiblePages - 2)
            {
                for (int i = 2; i <= maxVisiblePages - 2; i++)
                {
                    pages.Add(i.ToString());
                }
                pages.Add("...");
                pages.Add(maxPage.ToString());
            }
            else if (currentPage >= maxPage - (maxVisiblePages - 2))
            {
                pages.Add("...");
                for (int i = maxPage - (maxVisiblePages - 2); i <= maxPage; i++)
                {
                    pages.Add(i.ToString());
                }
            }
            else
            {
                pages.Add("...");
                int startPage = currentPage - (maxVisiblePages - 4) / 2;
                int endPage = currentPage + (maxVisiblePages - 4) / 2;
                for (int i = startPage; i <= endPage; i++)
                {
                    pages.Add(i.ToString());
                }
                pages.Add("...");
                pages.Add(maxPage.ToString());
            }
        }

        return pages;
    }
}