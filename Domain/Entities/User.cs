namespace RestaurantReservation.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using RestaurantReservation.Domain.Enums;

/// <summary>
/// Domain entity representing an application user.
/// </summary>
public class User
{
    /// <summary>Unique identifier for the user (typically Identity user id).</summary>
    public string Id { get; set; } = null!;

    /// <summary>User email address.</summary>
    public string Email { get; set; } = null!;

    /// <summary>Hashed password for the user.
    /// Note: storing plain-text passwords is not supported; hashing handled by Identity.</summary>
    public string Password { get; set; } = null!;

    /// <summary>User first name.</summary>
    public string FirstName { get; set; } = null!;

    /// <summary>User last name (optional).</summary>
    public string? LastName { get; set; }

    /// <summary>Contact phone number.</summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>Timestamp when the user was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>User role (stored as nvarchar(20)).</summary>
    [Column(TypeName = "nvarchar(20)")]
    public UserRole Role { get; set; } = UserRole.Customer;

    /// <summary>Reservations made by the user.</summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}