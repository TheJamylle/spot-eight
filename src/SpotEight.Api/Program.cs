using SpotEight.Api;
using SpotEight.Api.Config;
using SpotEight.Core.Application.Middlewares;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Logger.AddSerilog(builder.Configuration);

    builder.Host.UseSerilog(Log.Logger);

    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestSerilLogMiddleware>();

    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    startup.Configure(app, app.Environment, provider);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
