using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Users.Events;

public sealed record UserChangedPasswordEvent(string Email) : DomainEvent;