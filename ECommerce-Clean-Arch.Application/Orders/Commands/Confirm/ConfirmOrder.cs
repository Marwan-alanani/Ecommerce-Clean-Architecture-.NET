using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Orders;
using ECommerce_Clean_Arch.Domain.Orders.Enums;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Orders.Commands.Confirm;

public sealed record ConfirmOrderCommand(OrderId OrderId) : ICommand;

public sealed class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTime;

    public ConfirmOrderCommandHandler(IApplicationDbContext context, IDateTimeProvider dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Where(o => o.Id == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);
        if (order is null)
        {
            return Error.NotFound(new OrderNotFound(request.OrderId));
        }

        if (order.Status == OrderStatus.Confirmed)
        {
            return Result.Success();
        }

        var confirmResult = order.Confirm(_dateTime.UtcNow);
        if (confirmResult.IsFailure)
        {
            return confirmResult;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}