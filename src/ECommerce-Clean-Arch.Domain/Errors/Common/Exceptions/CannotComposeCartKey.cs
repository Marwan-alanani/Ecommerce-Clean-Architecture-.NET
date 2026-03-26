namespace ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;

public class CannotComposeCartKey() : Exception(
    "Cannot compose cart key ... missing guest id");