using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// Used to create a reservation in a restaurant.
/// </summary>
public class CreateReservationDto
{
    [Required]
    public DateTime ReservationDate { get; set; }

    [Required]
    [Range(1, 20)]
    public int NumberOfGuests { get; set; }

    public string? SpecialRequests { get; set; }

    [Required]
    public int RestaurantId { get; set; }
}

/// <summary>
/// Used to get the infos on a reservation.
/// </summary>
public class ReservationDto
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? SpecialRequests { get; set; }
    public DateTime ReservationTime { get; set; }

    // Related data
    public string RestaurantName { get; set; } = string.Empty;
    public string TableNumber { get; set; } = string.Empty;
    public string CustomerFullName { get; set; } = string.Empty;
}

/// <summary>
/// Used to check if a reservation in a restaurant is possible at a specific time.
/// </summary>
public class ReservationAvailabilityDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    [Range(1, 20)]
    public int NumberOfGuests { get; set; }

    [Required]
    public int RestaurantId { get; set; }
}