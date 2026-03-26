using ECommerce_Clean_Arch.Domain.Users;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Users;

public sealed record UserEmailNotConfirmed : ErrorReason
{
    public UserEmailNotConfirmed(string email) : base(
        nameof(UserEmailNotConfirmed),
        $"User email has not yet been confirmed check your mail:{email} to confirm",
        nameof(User.Email)
    )
    {
    }
}