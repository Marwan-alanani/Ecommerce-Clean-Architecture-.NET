using ECommerce_Clean_Arch.Application.Authentication.Interfaces;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Infrastructure.Authentication;
using ECommerce_Clean_Arch.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECommerce_Clean_Arch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfigurationManager config
    )
    {
        services
            .AddPersistence(config)
            .AddAuthentication(config)
            .AddServices();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfigurationManager config
    )
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("IdentityDb");
            options.UseNpgsql(connectionString);
        });

        services.AddIdentityCore<User>(builder =>
            {
                builder.User.RequireUniqueEmail = true;
                builder.Password.RequiredLength = 6;
                builder.Password.RequireDigit = true;
                builder.Password.RequiredUniqueChars = 3;
                builder.Password.RequireLowercase = true;
                builder.Password.RequireUppercase = true;
                builder.Password.RequireNonAlphanumeric = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>();
        return services;
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfigurationManager config
    )
    {
        var jwtConfig = new JwtConfig();
        config.Bind(JwtConfig.SectionName, jwtConfig);
        services.AddSingleton(Options.Create(jwtConfig));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}