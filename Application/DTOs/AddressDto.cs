using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// Used to create an address for a restaurant.
/// </summary>
public class CreateAddressDto
{
    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public int ZipCode { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    public int RestaurantId { get; set; }
}

/// <summary>
/// Used to transfer address information.
/// </summary>
public class AddressDto
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public int ZipCode { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
}
