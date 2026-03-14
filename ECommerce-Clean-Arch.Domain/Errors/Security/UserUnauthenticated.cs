using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Security;

public sealed record UserUnauthenticated() : ErrorReason(
    ErrorCodes.UserUnauthenticated,
    "User is not authenticated"
);