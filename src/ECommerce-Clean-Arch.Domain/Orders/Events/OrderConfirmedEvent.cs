using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Orders.Events;

public record OrderConfirmedEvent (Guid UserId): DomainEvent;