using AutoMapper;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Interfaces;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.AspNetCore.Identity;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public record Login(
    string Email,
    string Password
) : IQuery<AuthenticationResult>;

public class LoginQueryHandler : IQueryHandler<Login, AuthenticationResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public LoginQueryHandler(
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        Login request,
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

        var token = await _jwtTokenGenerator.Generate(user);
        return _mapper.Map<AuthenticationResult>((user, token));
    }
}