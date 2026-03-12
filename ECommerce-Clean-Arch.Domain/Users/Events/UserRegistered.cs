using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Users.Events;

public record UserRegistered(
    Guid AggregateId,
    string Email,
    string UserName,
    long AggregateVersion
) : IDomainEvent; // could've used INotification directly wouldn't really matter