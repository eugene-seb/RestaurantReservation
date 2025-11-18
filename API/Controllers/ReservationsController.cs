using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces;

namespace RestaurantReservation.API.Controllers;

    /// <summary>
    /// Controller that exposes reservation-related endpoints for authenticated users.
    /// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new reservation for the authenticated user.
    /// </summary>
    /// <param name="createReservationDto">Reservation creation data.</param>
    /// <returns>The created reservation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto createReservationDto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservationDto = await _reservationService.CreateReservationAsync(createReservationDto, userId);

            return CreatedAtAction(nameof(GetUserReservations), new { reservationId = reservationDto.Id }, reservationDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get all reservations belonging to the authenticated user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var reservations = await _reservationService.GetUserReservationsAsync(userId);

        return Ok(reservations);
    }

    /// <summary>
    /// Get a single reservation by id for the authenticated user.
    /// </summary>
    /// <param name="reservationId">Reservation identifier.</param>
    [HttpGet("{reservationId:int}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> GetUserReservations(int reservationId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var reservation = await _reservationService.GetReservationByIdAsync(reservationId, userId);
        if (reservation is null)
            return NotFound();

        return Ok(reservation);
    }

    /// <summary>
    /// Cancel a reservation owned by the authenticated user.
    /// </summary>
    /// <param name="reservationId">Reservation identifier to cancel.</param>
    [HttpPut("{reservationId:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelReservation(int reservationId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _reservationService.CancelReservationAsync(reservationId, userId);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Check table availability for a requested reservation window.
    /// </summary>
    /// <param name="availabilityDto">Availability query parameters.</param>
    /// <returns>List of available tables for the requested window.</returns>
    [HttpGet("availability")]
    [ProducesResponseType(typeof(IEnumerable<TableDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TableDto>>> CheckAvailability([FromQuery] ReservationAvailabilityDto availabilityDto)
    {
        try
        {
            var tablesAvailable = await _reservationService.CheckAvailabilityAsync(availabilityDto);

            return Ok(tablesAvailable);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }

    }
}

