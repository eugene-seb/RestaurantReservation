namespace RestaurantReservation.Domain.Entities;

/// <summary>
/// Domain entity representing a table inside a restaurant.
/// </summary>
public class Table
{
    /// <summary>Table identifier.</summary>
    public int Id { get; set; }

    /// <summary>Number identifying the table within the restaurant.</summary>
    public int TableNumber { get; set; }

    /// <summary>Seating capacity of the table.</summary>
    public int Capacity { get; set; }

    /// <summary>Foreign key to the owning restaurant.</summary>
    public int RestaurantId { get; set; }

    /// <summary>Navigation to the owning restaurant.</summary>
    public virtual Restaurant Restaurant { get; set; } = null!;

    /// <summary>Reservations associated with this table.</summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}