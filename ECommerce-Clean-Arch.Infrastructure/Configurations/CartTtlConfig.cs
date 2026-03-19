namespace ECommerce_Clean_Arch.Infrastructure.Configurations;

public sealed class CartTtlConfig
{
    public const string  SectionName = "CartTtl";
    public int GuestTtlDays { get; init; }
    public int UserTtlDays { get; init; }
}