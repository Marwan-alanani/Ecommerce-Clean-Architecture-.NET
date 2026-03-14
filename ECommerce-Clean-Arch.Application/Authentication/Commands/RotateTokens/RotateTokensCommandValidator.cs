using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RotateTokens;

public sealed class RotateTokensCommandValidator : AbstractValidator<RotateTokensCommand>
{
    public RotateTokensCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}