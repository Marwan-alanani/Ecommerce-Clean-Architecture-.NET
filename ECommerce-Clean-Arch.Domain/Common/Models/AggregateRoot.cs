namespace ECommerce_Clean_Arch.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : IEquatable<TId>
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }
}