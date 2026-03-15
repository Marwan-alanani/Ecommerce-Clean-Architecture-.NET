using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Domain.Users.Events;

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
        return identityResult.ToResult();
    }

    public async Task<Result<User>> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null
            ||
            !(await _userManager.CheckPasswordAsync(user, password))
            ||
            !user.IsEnabled)
        {
            return Error.Validation(new InvalidCredentials());
        }


        return user;
    }

    public async Task<Result> ChangePasswordAsync(
        User user,
        string currentPassword,
        string newPassword
    )
    {
        var identityResult = await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword);
        user.AddDomainEvent(new UserChangedPassword(user.Email!));
        return identityResult.ToResult();
    }
}

public static class IdentityResultExtensions
{
    public static Result ToResult(this IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            var error = Error.Validation();
            foreach (var validationError in identityResult.Errors)
                error.AddReason(validationError.Code, validationError.Description);

            return error;
        }

        return Result.Success();
    }
}