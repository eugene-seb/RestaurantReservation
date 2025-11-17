using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// DTO used when creating an address for a restaurant.
/// </summary>
public class CreateAddressDto
{
    /// <summary>Street address (required).</summary>
    [Required]
    public string Street { get; set; } = string.Empty;

    /// <summary>Postal/ZIP code (required).</summary>
    [Required]
    public int ZipCode { get; set; }

    /// <summary>City name (required).</summary>
    [Required]
    public string City { get; set; } = string.Empty;

    /// <summary>Country name (required).</summary>
    [Required]
    public string Country { get; set; } = string.Empty;

    /// <summary>Foreign key to the restaurant this address belongs to.</summary>
    public int RestaurantId { get; set; }
}

/// <summary>
/// DTO used to transfer address information.
/// </summary>
public class AddressDto
{
    /// <summary>Address identifier.</summary>
    public int Id { get; set; }

    /// <summary>Street address.</summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>Postal/ZIP code.</summary>
    public int ZipCode { get; set; }

    /// <summary>City name.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Country name.</summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>Associated restaurant identifier.</summary>
    public int RestaurantId { get; set; }
}

/// <summary>
/// DTO used when updating an address.
/// </summary>
public class UpdateAddressDto
{
    /// <summary>Street address (required).</summary>
    [Required]
    public string Street { get; set; } = string.Empty;

    /// <summary>Postal/ZIP code (required).</summary>
    [Required]
    public int ZipCode { get; set; }

    /// <summary>City name (required).</summary>
    [Required]
    public string City { get; set; } = string.Empty;

    /// <summary>Country name (required).</summary>
    [Required]
    public string Country { get; set; } = string.Empty;

    /// <summary>Associated restaurant identifier.</summary>
    public int RestaurantId { get; set; }
}
