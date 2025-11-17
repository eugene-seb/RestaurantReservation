using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// DTO used to create a table in a restaurant.
/// </summary>
public class CreateTableDto
{
    /// <summary>Number identifying the table within the restaurant (required).</summary>
    [Required]
    public int TableNumber { get; set; }

    /// <summary>Seating capacity of the table (required).</summary>
    [Required]
    [Range(1, 20)]
    public int Capacity { get; set; }

    /// <summary>Associated restaurant identifier (required).</summary>
    [Required]
    public int RestaurantId { get; set; }
}

/// <summary>
/// DTO representing table information.
/// </summary>
public class TableDto
{
    /// <summary>Table identifier.</summary>
    public int Id { get; set; }

    /// <summary>Number identifying the table within the restaurant.</summary>
    public int TableNumber { get; set; }

    /// <summary>Seating capacity of the table.</summary>
    public int Capacity { get; set; }

    /// <summary>Associated restaurant identifier.</summary>
    public int RestaurantId { get; set; }

    /// <summary>Restaurant name for convenience when returning joined results.</summary>
    public string RestaurantName { get; set; } = string.Empty;
}

/// <summary>
/// DTO used to update a table.
/// </summary>
public class UpdateTableDto
{
    /// <summary>Number identifying the table within the restaurant (required).</summary>
    [Required]
    public int TableNumber { get; set; }

    /// <summary>Seating capacity of the table (required).</summary>
    [Required]
    [Range(1, 100)]
    public int Capacity { get; set; }

    /// <summary>Associated restaurant identifier (required).</summary>
    [Required]
    public int RestaurantId { get; set; }
}

