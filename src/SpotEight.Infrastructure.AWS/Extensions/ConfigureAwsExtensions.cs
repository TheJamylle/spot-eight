using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotEight.Core.Domain.Interfaces.Services;
using SpotEight.Infrastructure.AWS.Configurations;
using SpotEight.Infrastructure.AWS.Services;

namespace SpotEight.Infrastructure.AWS.Extensions;

public static class ConfigureAwsExtensions
{
    public static IServiceCollection AddAwsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsConfig>(configuration.GetSection("AwsConfig"));

        services.AddScoped<IStorageServices, S3Services>();

        return services;
    }
}
