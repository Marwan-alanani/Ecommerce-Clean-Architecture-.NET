namespace ECommerce_Clean_Arch.Infrastructure.Configurations;

public class StripeConfig
{
    public const string SectionName = "Stripe";
    public string SuccessUrl { get; init; } = null!;
    public string CancelUrl { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string WebhookKey { get; init; } = null!;
}