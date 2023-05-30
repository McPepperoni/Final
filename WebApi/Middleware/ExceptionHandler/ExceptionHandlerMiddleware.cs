using System.Net;
using Newtonsoft.Json;

namespace WebApi.Middleware.ExceptionHandler;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;

            switch (ex)
            {
                case AppException e:
                    response.StatusCode = (int)e.StatusCode;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(ex, "An exception has occur.");
                    break;
            }

            var result = JsonConvert.SerializeObject(new { message = ex.Message });
            await response.WriteAsync(result);
        }
    }
}