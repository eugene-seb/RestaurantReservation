namespace RestaurantReservation.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = null!;
    public int ZipCode { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}