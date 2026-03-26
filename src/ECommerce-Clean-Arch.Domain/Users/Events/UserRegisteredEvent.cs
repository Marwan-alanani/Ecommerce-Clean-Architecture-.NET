using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Users.Events;

public record UserRegisteredEvent(string Email, string UserName) : DomainEvent;