namespace RestaurantReservation.Domain.Entities;

/// <summary>
/// Domain entity representing a postal address.
/// </summary>
public class Address
{
    /// <summary>Address identifier.</summary>
    public int Id { get; set; }

    /// <summary>Street address.</summary>
    public string Street { get; set; } = null!;

    /// <summary>Postal/ZIP code.</summary>
    public int ZipCode { get; set; }

    /// <summary>City name.</summary>
    public string City { get; set; } = null!;

    /// <summary>Country name.</summary>
    public string Country { get; set; } = null!;

    /// <summary>Foreign key to the restaurant this address belongs to.</summary>
    public int RestaurantId { get; set; }

    /// <summary>Navigation property back to the restaurant.</summary>
    public virtual Restaurant Restaurant { get; set; } = null!;
}