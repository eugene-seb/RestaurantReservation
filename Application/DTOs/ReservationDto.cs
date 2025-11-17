using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// DTO used to create a reservation in a restaurant.
/// </summary>
public class CreateReservationDto
{
    /// <summary>Name for the reservation (required).</summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>Full date and time of the reservation (required).</summary>
    [Required]
    public DateTime ReservationDate { get; set; }

    /// <summary>Number of guests for the reservation (required).</summary>
    [Required]
    [Range(1, 20)]
    public int NumberOfGuests { get; set; }

    /// <summary>Optional special requests from the guest.</summary>
    public string? SpecialRequests { get; set; }

    /// <summary>Identifier of the restaurant where the reservation is requested (required).</summary>
    [Required]
    public int RestaurantId { get; set; }
}

/// <summary>
/// DTO representing reservation details returned to clients.
/// </summary>
public class ReservationDto
{
    /// <summary>Reservation identifier.</summary>
    public int Id { get; set; }

    /// <summary>Name on the reservation.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Requested reservation date and time.</summary>
    public DateTime ReservationDate { get; set; }

    /// <summary>Number of guests.</summary>
    public int NumberOfGuests { get; set; }

    /// <summary>Reservation status (e.g., Confirmed, Cancelled).</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Optional special requests.</summary>
    public string? SpecialRequests { get; set; }

    /// <summary>Normalized reservation time (for display).</summary>
    public DateTime ReservationTime { get; set; }

    /// <summary>Name of the restaurant where the reservation is held.</summary>
    public string RestaurantName { get; set; } = string.Empty;

    /// <summary>Table number assigned to the reservation.</summary>
    public string TableNumber { get; set; } = string.Empty;
}

/// <summary>
/// DTO used to check availability for a given date/time and guest count.
/// </summary>
public class ReservationAvailabilityDto
{
    /// <summary>Desired reservation date and time (required).</summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>Number of guests to check availability for (required).</summary>
    [Required]
    [Range(1, 20)]
    public int NumberOfGuests { get; set; }

    /// <summary>Target restaurant identifier (required).</summary>
    [Required]
    public int RestaurantId { get; set; }
}