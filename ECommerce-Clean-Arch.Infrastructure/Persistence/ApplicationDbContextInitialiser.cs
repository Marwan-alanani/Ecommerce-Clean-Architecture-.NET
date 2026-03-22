using System.Security.Claims;

using ECommerce_Clean_Arch.Domain.Categories;
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

        // await initialiser.InitialiseAsync();
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

    private async Task TrySeedAsync()
    {
        await TrySeedIdentity();
        // seed other data here
        await TrySeedCategory();
    }

    private async Task TrySeedIdentity()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        // Default roles
        foreach (var roleName in Roles.GetAll())
        {
            var role = new Role() { Name = roleName };
            if (!roles.Contains(role))
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        var adminRole = await _roleManager.Roles
            .Where(role => role.Name == Roles.Admin)
            .FirstOrDefaultAsync();
        var adminClaims = await _roleManager.GetClaimsAsync(adminRole!);
        // inject Permissions for admin Role
        foreach (var policy in Permissions.GetAll())
        {
            var claim = new Claim(Permissions.ClaimType, policy);
            if (!adminClaims.Contains(claim))
                await _roleManager.AddClaimAsync(adminRole!, claim);
        }


        // Default users
        var administrator = User.Create(
            "admin",
            "admin",
            "admin",
            "admin@mail.com");

        if ((await _userManager.FindByEmailAsync("admin@mail.com") == null))
        {
            await _userManager.CreateAsync(administrator, "P@ssw0rd");
            await _userManager.AddToRolesAsync(administrator, [Roles.Admin]);
        }

        var user = User.Create(
            "user",
            "user",
            "user",
            "user@mail.com");

        if ((await _userManager.FindByEmailAsync("user@mail.com") == null))
        {
            await _userManager.CreateAsync(user, "P@ssw0rd");
            await _userManager.AddToRolesAsync(user, [Roles.User]);
        }
    }

    private async Task TrySeedCategory()
    {
        var category = Category.Create(
            "Unlisted"
        );
        var categoryNames = await _context.Categories.Select(c => c.Name).ToListAsync();
        if (!categoryNames.Contains(category.Name))
        {
            await _context.Categories.AddAsync(category);
        }

        await _context.SaveChangesAsync();
    }
}