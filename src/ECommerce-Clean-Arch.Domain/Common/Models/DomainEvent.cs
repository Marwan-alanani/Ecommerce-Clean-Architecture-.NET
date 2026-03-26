using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Common.Models;

public abstract record DomainEvent : IDomainEvent
{
    public long AggregateVersion { get; set; }
    public Guid AggregateId { get; set; }
}