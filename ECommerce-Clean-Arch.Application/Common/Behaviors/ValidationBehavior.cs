using FluentValidation;

using MediatR;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        // validate
        var validationResults = await Task.WhenAll(
            _validators.Select(validator =>
                validator.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken)
            )
        );
        var validationErrors = validationResults
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();

        if (validationErrors.Any())
        {
            var error = Error.Validation();
            foreach (var validationError in validationErrors)
            {
                error.AddReason(
                    new ErrorReason(
                        validationError.ErrorCode,
                        validationError.ErrorMessage,
                        validationError.PropertyName)
                );
            }

            return (dynamic)error;
        }


        return await next(cancellationToken);
    }
}