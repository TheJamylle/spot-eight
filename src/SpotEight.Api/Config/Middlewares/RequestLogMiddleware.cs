using System.Text;
using SpotEight.Core.Application.Common.Log;

namespace SpotEight.Api.Config.Middlewares;

/// <summary>
/// BadRequest json middleware
/// </summary>
public class RequestLogMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    public RequestLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Middleware invoker
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();
        await _next(context);
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        try
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 10240, true);

            var requestBody = await reader.ReadToEndAsync();

            var urlParams = $"{context.Request.Path}{context.Request.QueryString}";

            this.LogRequest(urlParams, requestBody, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            this.LogError($"Error reading request: {ex.Message}");
        }
        finally
        {
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

    }

}

/// <summary>
/// Class to inject middleware
/// </summary>
public static class RequestLogMiddlewareExtensions
{
    /// <summary>
    /// Middleware injector
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseRequestLog(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLogMiddleware>();
    }
}
