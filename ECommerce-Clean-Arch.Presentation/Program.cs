using DotNetEnv;

using ECommerce_Clean_Arch.Application;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Infrastructure;
using ECommerce_Clean_Arch.Infrastructure.Authentication.Services;
using ECommerce_Clean_Arch.Infrastructure.Persistence;
using ECommerce_Clean_Arch.Presentation.Errors;

using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ECommerce_Clean_Arch.Presentation;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        await Task.CompletedTask;
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder.Services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
        builder.Services.AddScoped<IUser, CurrentUser>();
        {
            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // await app.InitialiseDatabaseAsync();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}