using MediatR;

namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IDomainEvent : INotification
{
    public long Version { get; }
    public Guid AggregateId { get; }
}