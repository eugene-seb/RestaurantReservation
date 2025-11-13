namespace RestaurantReservation.Domain.Entities;

using RestaurantReservation.Domain.Enums;

public class User
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public UserRole Role { get; set; } = UserRole.Customer;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}