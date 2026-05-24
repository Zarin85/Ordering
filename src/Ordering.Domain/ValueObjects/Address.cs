using Ordering.Domain.Common;

namespace Ordering.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string Country { get; }

    private Address(string street, string city, string country)
    {
        Street = street;
        City = city;
        Country = country;
    }

    public static Address Of(string street, string city, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street is required.");
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required.");
        if (string.IsNullOrWhiteSpace(country))
            throw new DomainException("Country is required.");
        return new Address(street, city, country);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return Country;
    }

    public override string ToString() => $"{Street}, {City}, {Country}";
}
