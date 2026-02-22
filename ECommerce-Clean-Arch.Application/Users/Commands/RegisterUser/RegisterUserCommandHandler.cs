using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication;
using ECommerce_Clean_Arch.Domain.Common;
using ECommerce_Clean_Arch.Domain.Errors.Common;
using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        UserManager<User> userManager
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
    }

    public async Task<Result<UserResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName,
            request.Email);

        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            var error = Error.Validation();
            foreach (var validationError in identityResult.Errors)
            {
                error.AddReason(validationError.Code, validationError.Description);
            }

            return error;
        }

        var token = _jwtTokenGenerator.Generate(user);
        return new UserResult(
            user.Id,
            user.UserName!,
            user.Email!,
            user.FirstName,
            user.LastName,
            token
        );
    }
}