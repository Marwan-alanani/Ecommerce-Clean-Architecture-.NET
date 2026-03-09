using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.AspNetCore.Identity;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;

    public IdentityService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> CreateAsync(User user, string password)
    {
        var identityResult = await _userManager.CreateAsync(user, password);
        if (!identityResult.Succeeded)
        {
            var error = Error.Validation();
            foreach (var validationError in identityResult.Errors)
                error.AddReason(validationError.Code, validationError.Description);

            return error;
        }

        return Result.Success();
    }

    public async Task<Result<User>> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null
            ||
            !(await _userManager.CheckPasswordAsync(user, password)))
        {
            return Error.Validation(new InvalidCredentials());
        }

        return user;
    }
}