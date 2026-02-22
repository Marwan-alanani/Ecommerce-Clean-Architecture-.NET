using ECommerce_Clean_Arch.Domain.Common;
using MediatR;

namespace ECommerce_Clean_Arch.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
    where TResponse : class
{
}