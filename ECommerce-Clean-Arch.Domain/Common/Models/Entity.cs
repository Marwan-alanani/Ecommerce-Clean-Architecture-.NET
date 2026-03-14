using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Common.Models;

public abstract class Entity<TId, TValue> : IEquatable<Entity<TId, TValue>>
    where TId : struct, IStronglyTypedId<TId, TValue>
    where TValue : IEquatable<TValue>
{
    public TId Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public bool Equals(Entity<TId, TValue>? other)
    {
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;
        return other.Id.Equals(Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        var other = (Entity<TId, TValue>)obj;
        return other.Id.Equals(Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TId, TValue>? left, Entity<TId, TValue>? right) =>
        Equals(left, right);

    public static bool operator !=(Entity<TId, TValue>? left, Entity<TId, TValue>? right) =>
        !Equals(left, right);
}