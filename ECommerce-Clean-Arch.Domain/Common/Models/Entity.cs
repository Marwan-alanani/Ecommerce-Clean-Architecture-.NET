namespace ECommerce_Clean_Arch.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : IEquatable<TId>
{
    public TId Id { get; protected set; } = default!;

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;
        return other.Id.Equals(Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        var other = (Entity<TId>)obj;
        return other.Id.Equals(Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !Equals(left, right);
}