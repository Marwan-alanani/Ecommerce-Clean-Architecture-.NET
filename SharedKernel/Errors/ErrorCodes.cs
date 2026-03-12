namespace SharedKernel.Errors;

public static class ErrorCodes
{
    public const string UserEmailFound = "UserEmailFound";
    public const string InvalidCredentials = "InvalidCredentials";
    public const string ProductNameExists = "ProductNameExists";
    public const string ProductNotFound = "ProductNotFound";
    public const string NoProductsFound = "NoProductsFound";
    public const string UserNotFound = "UserNotFound";
    public const string TokenExpired = "TokenExpired";
    public const string TokenReuseDetected = "TokenReuseDetected";
    public const string MissingTokenCookie = "MissingTokenCookie";
}