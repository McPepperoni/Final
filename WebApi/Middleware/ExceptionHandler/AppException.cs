using System.Globalization;
using System.Net;

namespace WebApi.Middleware.ExceptionHandler;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public AppException(HttpStatusCode statusCode) : base()
    {
        StatusCode = statusCode;
    }

    public AppException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public AppException(HttpStatusCode statusCode, string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
        StatusCode = statusCode;
    }
}