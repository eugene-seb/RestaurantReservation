namespace RestaurantReservation.Domain.Entities;

using RestaurantReservation.Domain.Enums;

public class Reservation
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public int nbrPersons { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

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
