using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.UserSessions.Events;

public record UserLoggedInEvent(string GuestId) : DomainEvent;