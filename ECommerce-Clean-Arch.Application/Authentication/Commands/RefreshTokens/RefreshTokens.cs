using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Security;
using ECommerce_Clean_Arch.Domain.Errors.Token;
using ECommerce_Clean_Arch.Domain.Errors.Users;
using ECommerce_Clean_Arch.Domain.UserSessions.Enums;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RefreshTokens;

public record RefreshTokensCommand : ICommand<string>;

public sealed class RefreshTokensCommandHandler
    : ICommandHandler<RefreshTokensCommand, string>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IApplicationDbContext _context;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUser _user;
    private readonly ICookieService _cookieService;

    public RefreshTokensCommandHandler(
        ITokenProvider tokenProvider,
        IDateTimeProvider dateTime,
        ISessionRepository sessionRepository,
        ICookieService cookieService,
        IUser user,
        IApplicationDbContext context
    )
    {
        _tokenProvider = tokenProvider;
        _sessionRepository = sessionRepository;
        _cookieService = cookieService;
        _user = user;
        _context = context;
    }

    public async Task<Result<string>> Handle(
        RefreshTokensCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_user.Id.HasValue)
        {
            return Error.Security(new UserUnauthenticated());
        }

        // 1. Get token
        var oldToken = _cookieService.GetRefreshToken();
        if (oldToken is null)
        {
            return Error.NotFound(new MissingTokenCookie());
        }

        // 2. Check if valid token
        var sessionData = await _sessionRepository.GetSessionDataByRefreshTokenAsync(
            oldToken,
            cancellationToken);
        if (sessionData is null)
        {
            return Error.Validation(new TokenNotFound(oldToken));
        }

        if (sessionData.UserId != _user.Id.Value)
        {
            await _sessionRepository.RevokeAllByUserIdAsync(
                _user.Id.Value,
                RevokedReason.SecurityBreach,
                cancellationToken);
            _cookieService.ClearRefreshToken();
            await _context.SaveChangesAsync(cancellationToken);
            return Error.Security(new SecurityBreach());
        }

        // 3. Remove old token and make a new one
        var user = await _context.Users.AsNoTracking()
            .Where(u => u.Id == _user.Id.Value)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Error.NotFound(new UserNotFound(_user.Id.Value));
        }

        var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);
        var newToken = _tokenProvider.GenerateOpaqueToken();

        await _sessionRepository.RefreshTokenAsync(
            oldToken,
            newToken,
            _user.Id.Value
        );
        _cookieService.ClearRefreshToken();
        _cookieService.SetRefreshToken(newToken);
        return accessToken;
    }
}