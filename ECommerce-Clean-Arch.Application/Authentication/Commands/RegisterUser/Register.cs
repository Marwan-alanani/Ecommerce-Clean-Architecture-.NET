
namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<Guid>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(
        IIdentityService identityService
    )
    {
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName,
            request.Email);
        var identityResult = await _identityService.CreateAsync(user, request.Password);
        if (identityResult.IsFailure)
        {
            return identityResult.Error;
        }


        return user.Id;
    }
}