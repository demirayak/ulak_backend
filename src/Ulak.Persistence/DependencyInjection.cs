using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ulak.Persistence.Context;

namespace Ulak.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        // DbContext
        services.AddDbContext<UlakDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            // Not: Do NOT call Serilog directly from persistence layer.
            // Let the host logging (configured in API/Program.cs) capture EF logs.
        });

        // Repositories
        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}