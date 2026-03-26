using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record TokenExpired() : ErrorReason(
    ErrorCodes.TokenExpired,
    "Refresh Token has expired. Please login again"
);