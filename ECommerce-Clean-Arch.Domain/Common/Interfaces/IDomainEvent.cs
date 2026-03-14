using MediatR;

namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IDomainEvent : INotification
{
    public long AggregateVersion { get; set; }
    public Guid AggregateId { get; set; }
}