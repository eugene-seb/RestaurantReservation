namespace RestaurantReservation.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReservation.Domain.Enums;

/// <summary>
/// Domain entity representing a reservation made by a user for a table.
/// </summary>
public class Reservation
{
    /// <summary>Reservation identifier.</summary>
    public int Id { get; set; }

    /// <summary>Name under which the reservation was made.</summary>
    public string Name { get; set; } = null!;

    /// <summary>Requested reservation date and time.</summary>
    public DateTime ReservationDate { get; set; }

    /// <summary>Number of guests for the reservation.</summary>
    public int NumberOfGuests { get; set; }

    /// <summary>Current status of the reservation (stored as nvarchar(20)).</summary>
    [Column(TypeName = "nvarchar(20)")]
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    /// <summary>Optional special requests provided by the guest.</summary>
    public string? SpecialRequests { get; set; }

    /// <summary>Normalized reservation time for display or tracking.</summary>
    public DateTime ReservationTime { get; set; } = DateTime.UtcNow;

    /// <summary>User identifier of the reservation owner.</summary>
    public string UserId { get; set; } = null!;

    /// <summary>Table identifier assigned to this reservation.</summary>
    public int TableId { get; set; }

    /// <summary>Navigation property to the user who made the reservation.</summary>
    public virtual UserAccount User { get; set; } = null!;

    /// <summary>Navigation property to the assigned table.</summary>
    public virtual Table Table { get; set; } = null!;

    /// <summary>Cancel the reservation. Throws if the reservation is already seated.</summary>
    public void Cancel()
    {
        if (Status == ReservationStatus.Seated)
            throw new InvalidOperationException("Cannot cancel a seated reservation");

        Status = ReservationStatus.Cancelled;
    }

    /// <summary>Indicates whether the reservation may be modified.
    /// Modifications are allowed when Pending or Confirmed.</summary>
    public bool CanBeModified()
    {
        return Status == ReservationStatus.Pending || Status == ReservationStatus.Confirmed;
    }
}
