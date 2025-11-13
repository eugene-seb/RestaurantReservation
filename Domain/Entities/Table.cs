namespace RestaurantReservation.Domain.Entities;

public class Table
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = null!;
    public int Capacity { get; set; }
    
    public virtual Restaurant Restaurant { get; set; } = null!;
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}