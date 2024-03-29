using System.Net;
using Serilog;

namespace WebApplicationAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadHttpRequestException badHttpRequestException)
            {
                Log.Error($"Message: {badHttpRequestException.Message}\nStack trace: {badHttpRequestException.StackTrace}");

                context.Response.StatusCode = badHttpRequestException.StatusCode;
                await context.Response.WriteAsync(badHttpRequestException.Message);
            }
            catch (Exception exception)
            {
                Log.Fatal($"Message: {exception.Message}\nStack trace: {exception.StackTrace}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
