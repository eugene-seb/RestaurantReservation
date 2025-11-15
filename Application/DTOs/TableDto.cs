using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// Used to create a table in a restaurant.
/// </summary>
public class CreateTableDto
{
    [Required]
    public int TableNumber { get; set; }

    [Required]
    [Range(1, 20)]
    public int Capacity { get; set; }

    [Required]
    public int RestaurantId { get; set; }
}

/// <summary>
/// Used to get table information.
/// </summary>
public class TableDto
{
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
}
