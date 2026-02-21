using ECommerce_Clean_Arch.Domain.Common;
using MediatR;

namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<Result<UserResult>>;