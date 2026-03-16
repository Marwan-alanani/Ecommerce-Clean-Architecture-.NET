using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Token;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.RefreshTokens;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RotateTokens;

public record RotateTokensCommand(
    string UserAgent,
    string? IpAddress
) : ICommand<string>;

public sealed class RotateTokensCommandHandler
    : ICommandHandler<RotateTokensCommand, string>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IDateTimeProvider _dateTime;
    private readonly IApplicationDbContext _context;
    private readonly ICookieService _cookieService;

    public RotateTokensCommandHandler(
        ITokenProvider tokenProvider,
        IDateTimeProvider dateTime,
        IRefreshTokenRepository tokenRepository,
        IApplicationDbContext context,
        ICookieService cookieService
    )
    {
        _tokenProvider = tokenProvider;
        _dateTime = dateTime;
        _tokenRepository = tokenRepository;
        _context = context;
        _cookieService = cookieService;
    }

    public async Task<Result<string>> Handle(
        RotateTokensCommand request,
        CancellationToken cancellationToken
    )
    {
        var tokenValue = _cookieService.GetRefreshToken();
        if (tokenValue is null)
        {
            return Error.NotFound(new MissingTokenCookie());
        }

        var oldToken = await _tokenRepository.GetByTokenValueAsync(tokenValue, cancellationToken);
        if (oldToken is null)
        {
            return Error.NotFound(new TokenNotFound(tokenValue));
        }

        if (oldToken.RevokedAt.HasValue)
        {
            // revoke all tokens for that user and return Error.Security
            await _tokenRepository.RevokeAllByUserIdAsync(
                oldToken.UserId,
                RevokedReason.SecurityBreach,
                cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Error.Security(new TokenReuseDetected(oldToken.UserId));
        }

        if (oldToken.IsExpired(_dateTime.UtcNow))
        {
            return Error.Validation(new TokenExpired());
        }

        var user = await _context.Users
            .Where(u => u.Id == oldToken.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            oldToken.Revoke(RevokedReason.UserDeleted, _dateTime.UtcNow);
        }

        else if (!user.IsActive)
        {
            oldToken.Revoke(RevokedReason.UserDeactivated, _dateTime.UtcNow);
        }

        if (user is null || !user.IsActive)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Error.NotFound(new UserNotFound(oldToken.UserId));
        }

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
        await _context.SaveChangesAsync(cancellationToken);
        _cookieService.SetRefreshToken(opaqueToken, refreshToken.ExpiresOnUtc);
        return await _tokenProvider.GenerateAccessToken(user);
    }
}