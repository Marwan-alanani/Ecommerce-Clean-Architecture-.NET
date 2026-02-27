using MediatR;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Abstractions.Messaging;

public interface IQuery<T> : IRequest<Result<T>>
{
}