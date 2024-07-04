using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using System.Diagnostics.CodeAnalysis;
namespace SpotEight.Api.Config;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public static class Logger
{
    /// <summary>
    /// 
    /// </summary>
    public static void AddSerilog(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("ApplicationName", $"API SpotEight - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}")
            .WriteTo.Async(wt => wt.Console(new JsonFormatter()))
            .CreateLogger();
    }
}
