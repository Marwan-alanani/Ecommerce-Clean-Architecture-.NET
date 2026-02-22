using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Interfaces;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries;

public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            var error = Error.Validation();
            error.AddReason(new InvalidCredentials());
            return error;
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
        {
            var error = Error.Validation();
            error.AddReason(new InvalidCredentials());
            return error;
        }

        var token = _jwtTokenGenerator.Generate(user);
        return new AuthenticationResult(user, token);
    }
}