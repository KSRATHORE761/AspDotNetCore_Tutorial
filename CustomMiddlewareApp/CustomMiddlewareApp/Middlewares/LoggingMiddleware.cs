using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomMiddlewareApp.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project

    public class LoggingMiddleware
    {
        private RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var startTime = DateTime.Now;
            Console.WriteLine($"starttime:{startTime.Millisecond}");
            await _next(httpContext);
            var endTime = DateTime.Now;
            Console.WriteLine($"endTime:{endTime.Millisecond}");
            var elapsedTime = endTime - startTime;
            var logMessage = $"{httpContext.Request.Method} {httpContext.Request.Path} {httpContext.Response.StatusCode} {elapsedTime.TotalMilliseconds}ms";
            Console.WriteLine(logMessage);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }

}
