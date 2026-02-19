using ECommerce_Clean_Arch.Application.Authentication;
using ECommerce_Clean_Arch.Application.Users.Errors;
using ECommerce_Clean_Arch.Domain.Users;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResult>>
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
        await Task.CompletedTask;

        var emailExists = _userManager.Users.Any(u => u.Email == request.Email);
        if (emailExists)
        {
            return new UserEmailFoundError(request.Email);
        }

        var user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName,
            request.Email);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errorCodes = result.Errors.Select(e => e.Code);
            var reasons = errorCodes.Select(errorCode => new Error(errorCode));
            var error = new Error("Cannot register user");
            error.CausedBy(reasons);
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