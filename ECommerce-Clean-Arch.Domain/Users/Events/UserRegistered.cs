using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Users.Events;

public record UserRegistered(string Email, string UserName) : IDomainEvent; // could've used
    // INotification directly wouldn't really matter