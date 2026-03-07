using ECommerce_Clean_Arch.Application.Authentication.Interfaces;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Infrastructure.Authentication;
using ECommerce_Clean_Arch.Infrastructure.BackgroundServices;
using ECommerce_Clean_Arch.Infrastructure.EventBus;
using ECommerce_Clean_Arch.Infrastructure.Persistence;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Interceptors;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;
using ECommerce_Clean_Arch.Infrastructure.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using IConnectionFactory = Microsoft.AspNetCore.Connections.IConnectionFactory;

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
        var connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin"
        };
        var rabbitMqEventBus = new RabbitMqEventBus(connectionFactory);
        services.AddSingleton(connectionFactory);
        services.AddScoped(sp => rabbitMqEventBus);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddHostedService<PublishOutboxMessages>();
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfigurationManager config
    )
    {
        services.AddScoped<ISaveChangesInterceptor, AuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var connectionString = config.GetConnectionString(ApplicationDbContext.ConnectionStringName);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
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
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ApplicationDbContextInitialiser>();
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
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}