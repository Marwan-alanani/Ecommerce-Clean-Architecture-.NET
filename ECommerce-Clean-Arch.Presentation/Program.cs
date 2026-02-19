using ECommerce_Clean_Arch.Application;
using ECommerce_Clean_Arch.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Presentation;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Configuration.AddEnvironmentVariables();
        {
            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var identityDbContext =
                    scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                // var applicationDbContext = scope.ServiceProvider
                //     .GetRequiredService<ApplicationDbContext>();
                identityDbContext.Database.Migrate();
                // applicationDbContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseHttpsRedirection();

        app.Run();
    }
}