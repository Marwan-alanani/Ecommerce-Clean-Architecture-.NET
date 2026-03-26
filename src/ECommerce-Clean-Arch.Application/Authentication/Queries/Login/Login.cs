

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
    private readonly IDateTimeProvider _dateTime;
    private readonly IApplicationDbContext _context;
    private readonly ICookieService _cookieService;

    public LoginQueryHandler(
        ITokenProvider tokenProvider,
        IIdentityService identityService,
        IDateTimeProvider dateTime,
        IConfiguration configuration,
        ISessionRepository sessionRepository,
        IApplicationDbContext context,
        ICookieService cookieService
    )
    {
        _tokenProvider = tokenProvider;
        _identityService = identityService;
        _dateTime = dateTime;
        _sessionRepository = sessionRepository;
        _context = context;
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

        var opaqueToken = _tokenProvider.GenerateOpaqueToken(64);

        var userSession = UserSession.Create(
            user.Id,
            request.UserAgent,
            request.IpAddress,
            _dateTime.UtcNow
        );

        var guestId = _cookieService.GetGuestSessionId();
        if (guestId is not null)
        {
            user.AddDomainEvent(new UserLoggedInEvent(guestId));
        }

        await _sessionRepository.AddAsync(
            userSession,
            opaqueToken,
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _cookieService.ClearGuestSession();
        _cookieService.SetRefreshToken(opaqueToken);
        return accessToken;
    }
}