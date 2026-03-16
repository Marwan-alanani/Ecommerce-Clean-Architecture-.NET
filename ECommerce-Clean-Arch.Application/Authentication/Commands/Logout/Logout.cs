using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

using Microsoft.Extensions.Logging;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.Logout;

public sealed record LogoutCommand : ICommand;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IApplicationDbContext _unitOfWork;
    private readonly IDateTimeProvider _dateTime;
    private readonly ICookieService _cookieService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IRefreshTokenRepository tokenRepository,
        IApplicationDbContext unitOfWork,
        IDateTimeProvider dateTime,
        ILogger<LogoutCommandHandler> logger,
        ICookieService cookieService
    )
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _logger = logger;
        _cookieService = cookieService;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var tokenValue = _cookieService.GetRefreshToken();
        if (tokenValue == null)
        {
            return Result.Success();
        }

        var token = await _tokenRepository.GetByTokenValueAsync(tokenValue, cancellationToken);
        if (token is null)
        {
            return Result.Success();
        }

        if (!token.RevokedAt.HasValue)
        {
            token.Revoke(RevokedReason.LoggedOut, _dateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        _cookieService.ClearRefreshToken();

        return Result.Success();
    }
}