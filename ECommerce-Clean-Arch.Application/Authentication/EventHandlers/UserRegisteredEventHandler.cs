using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Domain.Users.Events;

using MediatR;

namespace ECommerce_Clean_Arch.Application.Authentication.EventHandlers;

public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        Console.WriteLine("------------------------------------------");
        Console.WriteLine(" WORKING!! ");
        Console.WriteLine("------------------------------------------");
    }
}