using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces;

namespace RestaurantReservation.API.Controllers;

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

            return CreatedAtAction(nameof(CreateReservation), new { id = reservationDto.Id }, reservationDto);
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


}

