using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record TokenReuseDetected(Guid UserId) : ErrorReason(
    ErrorCodes.TokenReuseDetected,
    $"Security violation detected. All sessions have been revoked for user {UserId}."
);