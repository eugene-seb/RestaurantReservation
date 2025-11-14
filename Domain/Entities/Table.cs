namespace RestaurantReservation.Domain.Entities;

public class Table
{
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }

    // Foreign key
    public int RestaurantId { get; set; }

    //Navigation properties
    public virtual Restaurant Restaurant { get; set; } = null!;
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}