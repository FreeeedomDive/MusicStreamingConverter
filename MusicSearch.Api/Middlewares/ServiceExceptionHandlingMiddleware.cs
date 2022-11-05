using MusicSearch.Dto.Exceptions;
using Newtonsoft.Json;

namespace MusicSearch.Api.Middlewares;

public class ServiceExceptionHandlingMiddleware
{
    public ServiceExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (MusicSearchApiException exception)
        {
            await WriteExceptionAsync(context, exception, exception.StatusCode);
        }
        catch (Exception exception)
        {
            await WriteExceptionAsync(context, exception, 500);
        }
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception ex, int statusCode)
    {
        var result = JsonConvert.SerializeObject(ex);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        await context.Response.WriteAsync(result);
    }

    private readonly RequestDelegate next;
}