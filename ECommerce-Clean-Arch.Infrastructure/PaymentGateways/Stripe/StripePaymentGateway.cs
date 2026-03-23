using ECommerce_Clean_Arch.Application.Orders;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;
using ECommerce_Clean_Arch.Infrastructure.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SharedKernel.Errors;
using SharedKernel.Results;

using Stripe;
using Stripe.Checkout;

namespace ECommerce_Clean_Arch.Infrastructure.PaymentGateways.Stripe;

public class StripePaymentGateway : IPaymentGateway
{
    private readonly StripeConfig _config;
    private readonly SessionService _sessionService;
    private readonly ILogger<StripePaymentGateway> _logger;

    public StripePaymentGateway(
        IOptions<StripeConfig> config,
        ILogger<StripePaymentGateway> logger,
        SessionService sessionService
    )
    {
        _logger = logger;
        _sessionService = sessionService;
        _config = config.Value;
    }

    public async Task<Result<CheckoutResult>> CreateCheckoutSession(
        OrderId orderId,
        List<CartItemData>
            items
    )
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            LineItems = items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = item.UnitPrice.Currency.ToLower(),
                        UnitAmount = (long)(item.UnitPrice.Amount * 100), // Stripe uses cents
                        ProductData =
                            new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name, Images = [item.PictureUrl]
                            }
                    },
                    Quantity = item.Quantity
                })
                .ToList(),
            Metadata = new Dictionary<string, string> { { "orderId", orderId.Value.ToString() } },
            SuccessUrl = _config.SuccessUrl,
            CancelUrl = _config.CancelUrl,
        };
        try
        {
            var session = await _sessionService.CreateAsync(options);
            return new CheckoutResult(session.Id, session.Url);
        }
        catch (StripeException ex)
        {
            // log also the error
            _logger.LogError(ex, ex.Message);
            return Error.InternalServerError(ex);
        }
    }
}