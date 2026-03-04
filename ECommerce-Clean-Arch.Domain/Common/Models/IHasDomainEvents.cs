namespace ECommerce_Clean_Arch.Domain.Common.Models;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    public void AddDomainEvent(IDomainEvent domainEvent);
}