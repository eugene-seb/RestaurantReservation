namespace RestaurantReservation.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReservation.Domain.Enums;

/// <summary>
/// Domain entity representing an application user.
/// </summary>
public class UserAccount
{
    /// <summary>Unique identifier for the user (from Identity user id).</summary>
    public string UserId { get; set; } = null!;

    /// <summary>User first name.</summary>
    public string FirstName { get; set; } = null!;

    /// <summary>User last name (optional).</summary>
    public string? LastName { get; set; }

    /// <summary>Timestamp when the user was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Reservations made by the user.</summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}