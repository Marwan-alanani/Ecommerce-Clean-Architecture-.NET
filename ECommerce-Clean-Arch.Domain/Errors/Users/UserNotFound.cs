using ECommerce_Clean_Arch.Domain.Users;
using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Users;

public sealed record UserNotFound(Guid Id) : ErrorReason(
    ErrorCodes.UserNotFound,
    $"User with the id:  '{Id}' was not found.",
    nameof(User.Id)
);