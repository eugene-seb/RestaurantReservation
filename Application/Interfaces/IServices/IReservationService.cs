using System;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Interfaces;

public interface IReservationService
{
    Task<ReservationDto> CreateReservationAsync(CreateReservationDto reservationDto, string userId);
    Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(string userId);
    Task<ReservationDto?> GetReservationByIdAsync(int reservationId, string userId);
    Task CancelReservationAsync(int id, string userId);
    Task<IEnumerable<TableDto>> CheckAvailabilityAsync(ReservationAvailabilityDto availabilityDto);

}
