using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Domain.Users;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<EntityCreatedDto>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, EntityCreatedDto>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(
        IIdentityService identityService
    )
    {
        _identityService = identityService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
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


        return new EntityCreatedDto(user.Id);
    }
}