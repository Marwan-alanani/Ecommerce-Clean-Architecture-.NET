using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.UserSessions;

using Microsoft.Extensions.Configuration;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password,
    string UserAgent,
    string? IpAddress
) : IQuery<string>;

public class LoginQueryHandler : IQueryHandler<LoginQuery, string>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentityService _identityService;
    private readonly ISessionRepository _sessionRepository;
    private readonly DateTime _utcNow;
    private readonly IApplicationDbContext _unitOfWork;
    private readonly ICookieService _cookieService;

    public LoginQueryHandler(
        ITokenProvider tokenProvider,
        IIdentityService identityService,
        IDateTimeProvider dateTime,
        IConfiguration configuration,
        ISessionRepository sessionRepository,
        IApplicationDbContext unitOfWork,
        ICookieService cookieService
    )
    {
        _tokenProvider = tokenProvider;
        _identityService = identityService;
        _utcNow = dateTime.UtcNow;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _cookieService = cookieService;
    }

    public async Task<Result<string>> Handle(
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

        var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);

        var opaqueToken = _tokenProvider.GenerateOpaqueToken();
        var userSession = UserSession.Create(
            user.Id,
            request.UserAgent,
            request.IpAddress,
            _utcNow
        );
        await _sessionRepository.AddAsync(
            userSession,
            opaqueToken,
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cookieService.SetRefreshToken(opaqueToken);
        return accessToken;
    }
}