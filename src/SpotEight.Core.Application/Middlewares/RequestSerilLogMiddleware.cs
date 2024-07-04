using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Diagnostics.CodeAnalysis;

namespace SpotEight.Core.Application.Middlewares;

[ExcludeFromCodeCoverage]
public class RequestSerilLogMiddleware
{
    private readonly RequestDelegate _next;

    public RequestSerilLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("x-correlation-id", context.Request.Headers["x-correlation-id"].ToString()))
        {
            return _next.Invoke(context);
        }
    }
}
