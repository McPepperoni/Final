using System.Text.RegularExpressions;

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
            nextPage += Regex.Replace(queryString, @"Page=\d*", $"Page={Page + 2}");
            prevPage += Regex.Replace(queryString, @"Page=\d*", $"Page={Page}");
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
}