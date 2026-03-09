using ECommerce_Clean_Arch.Domain.Users;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Services;

public interface IIdentityService
{
    Task<Result> CreateAsync(User user, string password);

    /// returns user if credentials are correct
    Task<Result<User>> AuthenticateAsync(string email, string password);
}