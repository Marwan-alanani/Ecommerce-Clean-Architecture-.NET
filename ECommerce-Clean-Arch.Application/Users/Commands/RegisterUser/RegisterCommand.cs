using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Users.Common;

namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<UserResult>;