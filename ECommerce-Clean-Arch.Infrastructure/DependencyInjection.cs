using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce_Clean_Arch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}