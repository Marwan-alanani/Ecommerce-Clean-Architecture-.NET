using MediatR;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<T> : IRequest<Result<T>>
{
}