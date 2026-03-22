using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Domain.Carts;

public sealed class ProductData
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string PictureUrl { get; init; } = null!;
    public MoneyFlat Price { get; init; } = null!;
}