using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Security;

public record SecurityBreach() : ErrorReason(
    nameof(SecurityBreach),
    "A security breach occured"
);