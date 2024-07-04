using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotEight.Core.Domain.Interfaces.Repository;
using SpotEight.Infrastructure.Database.Context;
using SpotEight.Infrastructure.Database.Repository;

namespace SpotEight.Infrastructure.Database.DependencyInjection;

public static class DatabaseServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (connectionString == null)
            throw new ArgumentNullException(connectionString, $"Variavel de ambiente {nameof(connectionString)} não foi encontrada");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUserRepository, UserRepository>();
    }
}
