using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.RefreshTokens;

using Microsoft.Extensions.Configuration;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password,
    string UserAgent,
    string? IpAddress
) : IQuery<AuthenticationResult>;

public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentityService _identityService;
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IDateTimeProvider _dateTime;
    private readonly IUnitOfWork _unitOfWork;

    public LoginQueryHandler(
        ITokenProvider tokenProvider,
        IIdentityService identityService,
        IDateTimeProvider dateTime,
        IConfiguration configuration,
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork
    )
    {
        _tokenProvider = tokenProvider;
        _identityService = identityService;
        _dateTime = dateTime;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
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

        var user = authenticationResult.Value;

        var accessToken = await _tokenProvider.GenerateAccessToken(user);

        var opaqueToken = _tokenProvider.GenerateOpaqueToken();
        var hash = _tokenProvider.HashOpaqueToken(opaqueToken);
        var refreshToken = RefreshToken.Create(
            user.Id,
            hash,
            _dateTime.UtcNow,
            request.UserAgent,
            request.IpAddress
        );
        await _tokenRepository.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);


        return new AuthenticationResult(
            accessToken,
            new RefreshTokenDto(opaqueToken, refreshToken.ExpiresOnUtc)
        );
    }
}