using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Security;

public sealed record UserUnauthorized : ErrorReason
{
    public UserUnauthorized() : base(
        nameof(UserUnauthorized),
        "User is not authorized to do action"
    )
    {
    }
}