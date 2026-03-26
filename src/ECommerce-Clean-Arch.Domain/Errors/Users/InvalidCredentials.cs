using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Users;

public record InvalidCredentials() :
    ErrorReason(ErrorCodes.InvalidCredentials, "Invalid email or " + "password !");