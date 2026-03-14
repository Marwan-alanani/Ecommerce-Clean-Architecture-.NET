namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IAggregateRootId<TSelf> : IStronglyTypedId<TSelf, Guid>
    where TSelf : struct, IEquatable<TSelf>
{
}