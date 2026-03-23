using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Orders.Enums;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Orders.Commands.Cancel;

public sealed record CancelOrderCommand(OrderId OrderId) : ICommand;

public sealed class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTime;

    public CancelOrderCommandHandler(IApplicationDbContext context, IDateTimeProvider dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Where(o => o.Id == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);
        if (order is null)
        {
            return Error.NotFound();
        }

        if (order.Status == OrderStatus.Cancelled)
        {
            return Result.Success();
        }

        var cancelResult = order.Cancel(_dateTime.UtcNow);
        if (cancelResult.IsFailure) return cancelResult;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}