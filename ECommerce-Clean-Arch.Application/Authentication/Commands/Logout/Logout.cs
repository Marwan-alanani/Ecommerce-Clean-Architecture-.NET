using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

using Microsoft.Extensions.Logging;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.Logout;

public sealed record LogoutCommand(string Token) : ICommand;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTime;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTime,
        ILogger<LogoutCommandHandler> logger
    )
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _logger = logger;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetByTokenValueAsync(request.Token, cancellationToken);
        if (token is null)
        {
            return Result.Success();
        }

        if (!token.RevokedAt.HasValue)
        {
            token.Revoke(RevokedReason.LoggedOut, _dateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}