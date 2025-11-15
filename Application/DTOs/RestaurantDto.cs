using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// Used to create a restaurant.
/// </summary>
public class CreateRestaurantDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    [Required]
    public TimeSpan OpeningTime { get; set; }

    [Required]
    public TimeSpan ClosingTime { get; set; }

    public CreateAddressDto? Address { get; set; }
}

/// <summary>
/// Used to get restaurant information.
/// </summary>
public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public AddressDto? RestaurantAddress { get; set; }
    public IList<TableDto> Tables { get; set; } = new List<TableDto>();
}
