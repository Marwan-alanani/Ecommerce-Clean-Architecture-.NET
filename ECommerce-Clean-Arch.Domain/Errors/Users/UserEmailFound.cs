using ECommerce_Clean_Arch.Domain.Errors.Common;
using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Domain.Errors.Users;

public record UserEmailFound : IReason
{
    public string Code => ErrorCodes.UserEmailFound;
    public string Description { get; }
    public string? Field => nameof(User.Email);

    public UserEmailFound(string email)
    {
        Description = $"User with the given email: {email} already exists";
    }
}