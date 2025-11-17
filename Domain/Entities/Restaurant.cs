namespace RestaurantReservation.Domain.Entities;

/// <summary>
/// Domain entity representing a restaurant.
/// </summary>
public class Restaurant
{
    /// <summary>Restaurant identifier.</summary>
    public int Id { get; set; }

    /// <summary>Restaurant name.</summary>
    public string Name { get; set; } = null!;

    /// <summary>Contact phone number (optional).</summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Daily opening time.</summary>
    public TimeSpan OpeningTime { get; set; }

    /// <summary>Daily closing time.</summary>
    public TimeSpan ClosingTime { get; set; }

    /// <summary>Address associated with the restaurant.</summary>
    public Address RestaurantAddress { get; set; } = null!;

    /// <summary>Collection of tables available in the restaurant.</summary>
    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

}