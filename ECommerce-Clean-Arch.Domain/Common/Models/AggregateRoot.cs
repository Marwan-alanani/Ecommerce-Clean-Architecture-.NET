using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : IEquatable<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList(); // return a copy
    public long Version { get; set; }

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}