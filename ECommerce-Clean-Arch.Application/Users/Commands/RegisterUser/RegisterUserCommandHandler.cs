using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Domain.Users;
using MediatR;

namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResult>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );
        _userRepository.Save(user);
        return new UserResult(
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            "Empty Token"
        );
    }
}