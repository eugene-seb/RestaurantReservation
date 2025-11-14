namespace RestaurantReservation.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReservation.Domain.Enums;

public class Reservation
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public int NumberOfGuests { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public string? SpecialRequests { get; set; }
    public DateTime ReservationTime { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public string UserId { get; set; } = null!;
    public int TableId { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Table Table { get; set; } = null!;

    public void Cancel()
    {
        if (Status == ReservationStatus.Seated)
            throw new InvalidOperationException("Cannot cancel a seated reservation");

        Status = ReservationStatus.Cancelled;
    }

    public bool CanBeModified()
    {
        return Status == ReservationStatus.Pending || Status == ReservationStatus.Confirmed;
    }
}
