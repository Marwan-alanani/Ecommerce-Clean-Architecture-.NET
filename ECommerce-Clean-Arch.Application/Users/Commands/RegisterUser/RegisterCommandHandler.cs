using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication;
using ECommerce_Clean_Arch.Application.Users.Common;
using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, UserResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        UserManager<User> userManager
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
    }

    public async Task<Result<UserResult>> Handle(
        RegisterCommand request,
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
                error.AddReason(validationError.Code, validationError.Description);

            return error;
        }

        var token = _jwtTokenGenerator.Generate(user);
        return new UserResult(
            user,
            token
        );
    }
}