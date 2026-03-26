using ECommerce_Clean_Arch.Application.Orders.Commands.Cancel;
using ECommerce_Clean_Arch.Application.Orders.Commands.Confirm;
using ECommerce_Clean_Arch.Domain.Errors.Orders;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;
using ECommerce_Clean_Arch.Infrastructure.Configurations;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using SharedKernel.Errors;
using SharedKernel.Results;

using Stripe;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("webhook")]
public sealed class WebhookController : ApiController
{
    private readonly ISender _sender;
    private readonly StripeConfig _stripeConfig;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        ISender sender,
        IOptions<StripeConfig> stripeConfig,
        ILogger<WebhookController> logger
    )
    {
        _sender = sender;
        _logger = logger;
        _stripeConfig = stripeConfig.Value;
    }

    [HttpPost("payment")]
    [AllowAnonymous]
    public async Task<IActionResult> Payment(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(HttpContext.Request.Body);
        var json = await reader.ReadToEndAsync(cancellationToken);

        var stripeSignature = Request.Headers["Stripe-Signature"];

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                _stripeConfig.WebhookKey);
        }
        catch (StripeException)
        {
            return BadRequest();
        }

        var result = stripeEvent.Type switch
        {
            EventTypes.PaymentIntentSucceeded =>
                await HandlePaymentIntentAsync(
                    stripeEvent,
                    orderId => _sender.Send(new ConfirmOrderCommand(orderId), cancellationToken)),

            EventTypes.PaymentIntentPaymentFailed =>
                await HandlePaymentIntentAsync(
                    stripeEvent,
                    orderId => _sender.Send(new CancelOrderCommand(orderId), cancellationToken)),

            _ => Result.Success()
        };

        if (result.IsFailure)
        {
            _logger.LogError("Webhook processing failed: {Error}", result.Error.Description);
        }

        return Ok();
    }

    private Result<OrderId> GetOrderId(PaymentIntent paymentIntent)
    {
        if (!paymentIntent.Metadata.TryGetValue("orderId", out var orderIdString)
            || string.IsNullOrEmpty(orderIdString))
            return Error.NotFound(new OrderIdNotFound());

        return OrderId.FromString(orderIdString);
    }

    private async Task<Result> HandlePaymentIntentAsync(
        Event stripeEvent,
        Func<OrderId, Task<Result>> invoke
    )
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

        if (paymentIntent is null)
        {
            return Error.Validation();
        }

        var orderId = GetOrderId(paymentIntent);
        if (orderId.IsFailure)
        {
            _logger.LogError(
                "Failed to extract orderId from PaymentIntent: {Error}",
                orderId.Error.Description
            );
            return Result.Success();
        }

        return await invoke(orderId.Value);
    }
}