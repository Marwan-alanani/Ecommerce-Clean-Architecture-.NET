using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Services;


using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IQuery<AuthenticationResult>;

public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IIdentityService _identityService;

    public LoginQueryHandler(
        IJwtTokenService jwtTokenService,
        IIdentityService identityService
    )
    {
        _jwtTokenService = jwtTokenService;
        _identityService = identityService;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken
    )
    {
        var authenticationResult = await _identityService
            .AuthenticateAsync(request.Email, request.Password);
        if (authenticationResult.IsFailure)
        {
            return authenticationResult.Error;
        }

        var token = await _jwtTokenService.Generate(authenticationResult.Value);
        return new AuthenticationResult(token);
    }
}