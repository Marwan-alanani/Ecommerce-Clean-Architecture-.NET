namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    public long Version { get; }
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    public void AddDomainEvent(IDomainEvent domainEvent);
}