using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Token;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.RefreshTokens;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RotateTokens;

public record RotateTokensCommand(
    string Token,
    string UserAgent,
    string? IpAddress
) : ICommand<AuthenticationResult>;

public sealed class RotateTokensCommandHandler
    : ICommandHandler<RotateTokensCommand, AuthenticationResult>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IDateTimeProvider _dateTime;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RotateTokensCommandHandler(
        ITokenProvider tokenProvider,
        IDateTimeProvider dateTime,
        IRefreshTokenRepository tokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    )
    {
        _tokenProvider = tokenProvider;
        _dateTime = dateTime;
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        RotateTokensCommand request,
        CancellationToken cancellationToken
    )
    {
        var oldToken = await _tokenRepository.GetByTokenValueAsync(request.Token, cancellationToken);
        if (oldToken is null)
        {
            return Error.NotFound(
                new ErrorReason(
                    "RefreshTokenNotFound",
                    "No refresh token is in db")
            );
        }

        if (oldToken.RevokedAt.HasValue)
        {
            // revoke all tokens for that user and return Error.Security
            await _tokenRepository.RevokeAllByUserIdAsync(
                oldToken.UserId,
                RevokedReason.SecurityBreach,
                cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Error.Security(new TokenReuseDetected(oldToken.UserId));
        }

        if (oldToken.IsExpired(_dateTime.UtcNow))
        {
            return Error.Validation(new TokenExpired());
        }

        var user = await _userRepository.GetUserByIdAsync(oldToken.UserId, cancellationToken);
        if (user is null)
        {
            oldToken.Revoke(RevokedReason.UserDeleted, _dateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Error.NotFound(new UserNotFound(oldToken.UserId));
        }

        var accessToken = await _tokenProvider.GenerateAccessToken(user);
        var opaqueToken = _tokenProvider.GenerateOpaqueToken();
        var opaqueTokenHash = _tokenProvider.HashOpaqueToken(opaqueToken);

        var refreshToken = RefreshToken.Create(
            user.Id,
            opaqueTokenHash,
            _dateTime.UtcNow,
            request.UserAgent,
            request.IpAddress);

        await _tokenRepository.AddAsync(refreshToken, cancellationToken);
        oldToken.Revoke(RevokedReason.TokenRotated, _dateTime.UtcNow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(
            accessToken,
            new RefreshTokenDto(opaqueToken, refreshToken.ExpiresOnUtc)
        );
    }
}