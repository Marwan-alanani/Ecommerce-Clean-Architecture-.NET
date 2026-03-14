using System.Security.Claims;

using ECommerce_Clean_Arch.Domain.Common.Security;
using ECommerce_Clean_Arch.Domain.Roles;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        await TrySeedIdentity();
        // seed other data here
    }

    public async Task TrySeedIdentity()
    {
        // Default roles
        foreach (var role in Roles.GetAll())
        {
            await _roleManager.CreateAsync(new Role { Name = role });
        }

        var adminRole = await _roleManager.Roles
            .Where(role => role.Name == Roles.Admin)
            .FirstOrDefaultAsync();
        // inject Permissions for admin Role
        foreach (var policy in Permissions.GetAll())
        {
            var claim = new Claim(Permissions.ClaimType, policy);
            await _roleManager.AddClaimAsync(adminRole!, claim);
        }


        // Default users
        var administrator = User.Create(
            "admin",
            "admin",
            "admin",
            "admin@mail.com");

        await _userManager.CreateAsync(administrator, "P@ssw0rd");
        await _userManager.AddToRolesAsync(administrator, [Roles.Admin]);
        var user = User.Create(
            "user",
            "user",
            "user",
            "user@mail.com");

        await _userManager.CreateAsync(user, "P@ssw0rd");
        await _userManager.AddToRolesAsync(user, [Roles.User]);
    }
}