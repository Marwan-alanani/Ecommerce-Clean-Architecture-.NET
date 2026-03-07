using AutoMapper;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Interfaces;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.AspNetCore.Identity;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;

public record Register(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<AuthenticationResult>;

public class RegisterCommandHandler : ICommandHandler<Register, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        UserManager<User> userManager,
        IMapper mapper
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        Register request,
        CancellationToken cancellationToken
    )
    {
        var user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName,
            request.Email);

        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            var error = Error.Validation();
            foreach (var validationError in identityResult.Errors)
                error.AddReason(validationError.Code, validationError.Description);

            return error;
        }

        var token = await _jwtTokenGenerator.Generate(user);
        return _mapper.Map<AuthenticationResult>((user, token));
    }
}