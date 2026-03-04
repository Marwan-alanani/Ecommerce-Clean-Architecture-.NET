using ECommerce_Clean_Arch.Domain.Users.Events;
using MediatR;

namespace ECommerce_Clean_Arch.Application.Authentication.EventHandlers;

public class UserRegisteredEventHandler : INotificationHandler<UserRegistered>
{
    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        Console.WriteLine("------------------------------------------");
        Console.WriteLine(" WORKING!! ");
        Console.WriteLine("------------------------------------------");
    }
}