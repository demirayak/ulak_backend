using Microsoft.Extensions.DependencyInjection;

namespace Ulak.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application servisleri burada register edilir
        return services;
    }
}