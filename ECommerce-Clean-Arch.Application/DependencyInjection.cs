using Microsoft.Extensions.DependencyInjection;

namespace ECommerce_Clean_Arch.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(Application.AssemblyReference).Assembly
            );
        });
        return services;
    }
}