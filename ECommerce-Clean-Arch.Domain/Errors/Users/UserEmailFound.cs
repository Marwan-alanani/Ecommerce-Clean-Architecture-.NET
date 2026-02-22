using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Users;

public record UserEmailFound : ErrorReason
{
    public UserEmailFound(string email) : base(
        ErrorCodes.UserEmailFound,
        $"User with the email: {email} was found!",
        nameof(email))
    {
    }
}