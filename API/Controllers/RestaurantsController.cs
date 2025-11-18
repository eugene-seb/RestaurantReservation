using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    /// <summary>
    /// Public endpoints for browsing restaurants. Administrative actions require an Administrator role.
    /// </summary>
    private readonly IRestaurantService _restaurantService;
    private readonly ILogger<RestaurantsController> _logger;

    public RestaurantsController(IRestaurantService restaurantService, ILogger<RestaurantsController> logger)
    {
        _restaurantService = restaurantService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve all restaurants.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<RestaurantDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
    {
        var restaurants = await _restaurantService.GetRestaurantsAsync();
        return Ok(restaurants);
    }

    /// <summary>
    /// Retrieve a restaurant by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantDto>> GetRestaurant(int id)
    {
        var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
        if (restaurant == null)
            return NotFound();

        return Ok(restaurant);
    }

    /// <summary>
    /// Search restaurants by name (case-insensitive, partial matches supported).
    /// </summary>
    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<RestaurantDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> SearchRestaurants([FromQuery] string name)
    {
        var restaurants = await _restaurantService.GetRestaurantsByNameAsync(name);
        return Ok(restaurants);
    }

    /// <summary>
    /// Create a new restaurant. Requires Administrator role.
    /// </summary>
    /// <param name="restaurantDto">Restaurant create DTO.</param>
    /// <returns>The created restaurant.</returns>
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    [ProducesResponseType(typeof(Restaurant), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Restaurant>> CreateRestaurant(CreateRestaurantDto restaurantDto)
    {
        try
        {
            var restaurant = await _restaurantService.CreateRestaurantAsync(restaurantDto);
            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, restaurant);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing restaurant. Requires Administrator role.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantDto restaurantDto)
    {
        try
        {
            await _restaurantService.UpdateRestaurantAsync(id, restaurantDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a restaurant. Requires Administrator role.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        try
        {
            await _restaurantService.DeleteRestaurantAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
