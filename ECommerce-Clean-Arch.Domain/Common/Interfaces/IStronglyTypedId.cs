namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IStronglyTypedId<TSelf, out TValue> : IEquatable<TSelf>
    where TValue : IEquatable<TValue>
    where TSelf : struct, IEquatable<TSelf>
{
    TValue Value { get; }
}