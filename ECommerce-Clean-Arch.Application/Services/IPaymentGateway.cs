using ECommerce_Clean_Arch.Application.Orders;
using ECommerce_Clean_Arch.Application.Orders.Commands.Checkout;
using ECommerce_Clean_Arch.Application.Orders.Commands.Checkout.Dtos;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Services;

public interface IPaymentGateway
{
    public Task<Result<CheckoutResult>> CreateCheckoutSession(OrderId orderId, List<CartItemData> items);
}