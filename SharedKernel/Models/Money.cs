namespace SharedKernel.Models;

public record Money
{
    // ReSharper disable once UnusedMember.Local
    private Money()
    {
    }

    public Money(Currency currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public Currency Currency { get; init; }
    public decimal Amount { get; init; }
}