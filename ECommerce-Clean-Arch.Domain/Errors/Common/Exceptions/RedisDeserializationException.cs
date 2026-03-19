namespace ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;

public sealed class RedisDeserializationException(string typeName) :
    Exception("There has been " + $"an error deserializing {typeName}");