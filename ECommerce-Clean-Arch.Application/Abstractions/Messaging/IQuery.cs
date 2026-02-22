using ECommerce_Clean_Arch.Domain.Common;
using MediatR;

namespace ECommerce_Clean_Arch.Application.Abstractions.Messaging
{
    public interface IQuery<T> : IRequest<Result<T>>
        where T : class
    {
    }
}