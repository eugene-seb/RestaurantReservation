using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    private readonly ILogger<RestaurantsController> _logger;

    public RestaurantsController(IRestaurantService restaurantService, ILogger<RestaurantsController> logger)
    {
        _restaurantService = restaurantService;
        _logger = logger;
    }

    
}

