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