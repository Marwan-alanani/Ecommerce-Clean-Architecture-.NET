using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

public sealed record ShippingAddress
{
    public string Street { get; }
    public string City { get; }
    public string Country { get; }
    public string PostalCode { get; }

    private ShippingAddress(
        string street,
        string city,
        string country,
        string postalCode
    )
    {
        Street = street;
        City = city;
        Country = country;
        PostalCode = postalCode;
    }

    public static Result<ShippingAddress> Create(
        string street,
        string city,
        string country,
        string postalCode
    )
    {
        if (string.IsNullOrWhiteSpace(street) ||
            string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(country) ||
            string.IsNullOrWhiteSpace(postalCode))
            return Error.Validation();

        return new ShippingAddress(
            street,
            city,
            country,
            postalCode);
    }
}