using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Orders.Commands.Checkout.Dtos;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Orders;
using ECommerce_Clean_Arch.Domain.Errors.Security;
using ECommerce_Clean_Arch.Domain.Orders;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Orders.Commands.Checkout;

// from the user ... I need the shipping address
public sealed record CheckoutCommand : ICommand<CheckoutResponse>
{
    public ShippingAddress ShippingAddress { get; init; } = null!;
}

public sealed class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, CheckoutResponse>
{
    private readonly IUser _user;
    private readonly IDateTimeProvider _dateTime;
    private readonly ICartRepository _cartRepository;
    private readonly ICartKeyResolver _cartKeyResolver;
    private readonly IApplicationDbContext _context;
    private readonly IPaymentGateway _paymentGateway;

    public CheckoutCommandHandler(
        IUser user,
        IDateTimeProvider dateTime,
        ICartRepository cartRepository,
        ICartKeyResolver cartKeyResolver,
        IApplicationDbContext context,
        IPaymentGateway paymentGateway
    )
    {
        _user = user;
        _dateTime = dateTime;
        _cartRepository = cartRepository;
        _cartKeyResolver = cartKeyResolver;
        _context = context;
        _paymentGateway = paymentGateway;
    }

    public async Task<Result<CheckoutResponse>> Handle(
        CheckoutCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_user.Id is null)
        {
            return Error.Security(new UserUnauthenticated());
        }

        var userId = _user.Id.Value;
        var cart = await _cartRepository.GetCartAsync(_cartKeyResolver.GetUserKey(userId));
        if (cart is null || !cart.Items.Any())
        {
            return Error.Validation(new EmptyCart());
        }

        // 1. Create an order object
        var orderResult = Order.Create(
            userId,
            request.ShippingAddress,
            cart,
            _dateTime.UtcNow
        );
        if (orderResult.IsFailure)
        {
            return orderResult.Error;
        }

        var order = orderResult.Value;
        // 2. Call payment gateway to get session url this needs the items in cart and orderId
        var cartItemData = cart.Items.Select(kv =>
            {
                var item = kv.Value;
                return new CartItemData()
                {
                    Name = item.Name,
                    PictureUrl = item.PictureUrl,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
            })
            .ToList();
        var checkoutResult = await _paymentGateway.CreateCheckoutSession(order.Id, cartItemData);
        if (checkoutResult.IsFailure)
        {
            return checkoutResult.Error;
        }

        order.SetSessionId(checkoutResult.Value.SessionId);

        // 3. Persist db changes
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        //4. Return OrderId , sessionUrl , PaymentId or orderId,sessionUrl
        return new CheckoutResponse(order.Id.Value, checkoutResult.Value.SessionUrl);
    }
}