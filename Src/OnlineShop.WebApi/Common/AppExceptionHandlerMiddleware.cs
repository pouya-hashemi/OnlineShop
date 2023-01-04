

using Newtonsoft.Json;
using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.WebApi.Common;


public class AppExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public AppExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (AppExceptionBase ex)
        {
            Console.WriteLine(ex);
            httpContext.Response.StatusCode = (int) ex.StatusCode;
            var result = new {message = ex.Message};
            var strResult = JsonConvert.SerializeObject(result);
            await httpContext.Response.WriteAsync(strResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            httpContext.Response.StatusCode = 500;
            var result = new {message = "Something Went Wrong"};
            var strResult = JsonConvert.SerializeObject(result);
            await httpContext.Response.WriteAsync(strResult);
        }
    }
}