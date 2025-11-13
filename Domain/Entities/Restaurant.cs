namespace RestaurantReservation.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public virtual Address? RestaurantAddress { get; set; }
    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

}