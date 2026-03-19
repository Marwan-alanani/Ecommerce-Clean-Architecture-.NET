using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId, Guid>, IAggregateRoot<TId>
    where TId : struct, IAggregateRootId<TId>
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }


    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList(); // return a copy
    public long Version { get; private set; }

    public bool IsActive { get; private set; } = true;

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent @event)
    {
        @event.AggregateId = Id.Value;
        @event.AggregateVersion = Version;
        _domainEvents.Add(@event);
        Version++;
    }


    public virtual void Deactivate()
    {
        IsActive = false;
    }

    public virtual void Activate()
    {
        IsActive = true;
    }
}