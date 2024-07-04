using FluentValidation.AspNetCore;
using SpotEight.Api.Config.Middlewares;
using SpotEight.Core.Application.DependencyInjection;
using SpotEight.Infrastructure.AWS.Extensions;
using SpotEight.Infrastructure.Database.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text.Json.Serialization;
using SpotEight.Api.Filters;
using SpotEight.Core.Application.Common.Http;
using SpotEight.Infrastructure.Database.Context;

namespace SpotEight.Api;

/// <summary>
/// 
/// </summary>
public class Startup
{
    /// <summary>
    /// 
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration) => Configuration = configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        services.AddApplication();

        services.AddAwsServices(Configuration);
        services.AddDatabase(Configuration);
        services.AddHealthCheck(Configuration);

        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();

        services.AddControllersWithViews(options =>
                {
                    options.Filters.Add<ApiExceptionFilterAttribute>();
                })
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddFluentValidation(x => x.AutomaticValidationEnabled = true);

        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        services.AddTransient<IBaseHttpService<object>, BaseHttpService<object>>();
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Staging";

        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"appsettings.{environment}.json");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="environment"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IApiVersionDescriptionProvider provider)
    {
        if (environment is null)
            throw new ArgumentNullException(nameof(environment));

        app.UseRequestLog();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions.Select(p => p.GroupName))
            {
                options.SwaggerEndpoint($"/swagger/{description}/swagger.json",
                    description.ToUpperInvariant());
            }

            options.DocExpansion(DocExpansion.List);
        });

        var scope = app.ApplicationServices.CreateScope();

        scope.ServiceProvider.GetService<ApplicationDbContext>()!.Database.Migrate();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials());

        app.UseHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
        });
    }
}

