using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Users.Events;

public sealed record UserChangedPassword(string Email) : DomainEvent;