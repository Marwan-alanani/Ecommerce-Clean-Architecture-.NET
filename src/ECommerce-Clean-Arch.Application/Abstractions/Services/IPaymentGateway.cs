

namespace ECommerce_Clean_Arch.Application.Abstractions.Services;

public interface IPaymentGateway
{
    public Task<Result<CheckoutResult>> CreateCheckoutSession(OrderId orderId, List<CartItemData> items);
}