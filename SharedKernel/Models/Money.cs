namespace SharedKernel.Models;

public record struct Money
{
    public Money(Currency currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public Currency Currency { get; init; }
    public decimal Amount { get; init; }
}