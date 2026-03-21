using System.Text;

using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Infrastructure.Authentication;
using ECommerce_Clean_Arch.Infrastructure.Authentication.Services;
using ECommerce_Clean_Arch.Infrastructure.BackgroundServices;
using ECommerce_Clean_Arch.Infrastructure.Configurations;
using ECommerce_Clean_Arch.Infrastructure.EventBus;
using ECommerce_Clean_Arch.Infrastructure.Persistence;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Interceptors;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;
using ECommerce_Clean_Arch.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using RabbitMQ.Client;

using StackExchange.Redis;

using Role = ECommerce_Clean_Arch.Domain.Roles.Role;


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
            .AddAuth(config)
            .AddConf(config)
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
        services.AddScoped(_ => rabbitMqEventBus);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ICartKeyResolver, CartKeyResolver>();
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
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!)
        );
        return services;
    }

    private static IServiceCollection AddConf(
        this IServiceCollection services,
        IConfigurationManager config
    )
    {
        services.Configure<CartTtlConfig>(config.GetSection(CartTtlConfig.SectionName));
        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfigurationManager config
    )
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<ICookieService, CookieService>();
        var jwtConfig = new JwtConfig();
        config.Bind(JwtConfig.SectionName, jwtConfig);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                };
            });
        services.AddAuthorization();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<JwtConfigOptionsSetup>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddScoped<IIdentityService, IdentityService>();


        return services;
    }
}