using System.Security.Cryptography;
using System.Text;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Users.Entities;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IQuery<AuthenticationResult>;

public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTime;

    public LoginQueryHandler(
        ITokenProvider tokenProvider,
        IIdentityService identityService,
        IUserRepository userRepository,
        IDateTimeProvider dateTime,
        IUnitOfWork unitOfWork
    )
    {
        _tokenProvider = tokenProvider;
        _identityService = identityService;
        _userRepository = userRepository;
        _dateTime = dateTime;
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

        var refreshTokenValue = _tokenProvider.GenerateRefreshToken();
        var hash = GenerateRefreshTokenHash(refreshTokenValue);
        // make it from configuration
        var refreshToken = RefreshToken.Create(hash, _dateTime.UtcNow.AddDays(7));

        user.AddRefreshToken(refreshToken);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(
            accessToken,
            new RefreshTokenDto(refreshTokenValue, refreshToken.ExpiresOnUtc)
        );
    }

    private string GenerateRefreshTokenHash(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}