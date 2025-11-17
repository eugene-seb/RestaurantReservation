using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// DTO used to create a restaurant.
/// </summary>
public class CreateRestaurantDto
{
    /// <summary>Restaurant name (required).</summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>Phone number (optional).</summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Opening time for the restaurant (required).</summary>
    [Required]
    public TimeSpan OpeningTime { get; set; }

    /// <summary>Closing time for the restaurant (required).</summary>
    [Required]
    public TimeSpan ClosingTime { get; set; }

    /// <summary>Optional nested address for the restaurant.</summary>
    public CreateAddressDto? Address { get; set; }
}

/// <summary>
/// DTO used to update a restaurant.
/// </summary>
public class UpdateRestaurantDto
{
    /// <summary>Restaurant name (required).</summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>Phone number (optional).</summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Opening time for the restaurant (required).</summary>
    [Required]
    public TimeSpan OpeningTime { get; set; }

    /// <summary>Closing time for the restaurant (required).</summary>
    [Required]
    public TimeSpan ClosingTime { get; set; }

    /// <summary>Optional nested address for the restaurant.</summary>
    public CreateAddressDto? Address { get; set; }
}

/// <summary>
/// DTO representing restaurant information returned to clients.
/// </summary>
public class RestaurantDto
{
    /// <summary>Restaurant identifier.</summary>
    public int Id { get; set; }

    /// <summary>Restaurant name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Phone number (optional).</summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Opening time.</summary>
    public TimeSpan OpeningTime { get; set; }

    /// <summary>Closing time.</summary>
    public TimeSpan ClosingTime { get; set; }

    /// <summary>Restaurant address information (optional).</summary>
    public AddressDto? RestaurantAddress { get; set; }

    /// <summary>List of tables available at the restaurant.</summary>
    public IList<TableDto> Tables { get; set; } = new List<TableDto>();
}
