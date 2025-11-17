using AutoMapper;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Services;

/// <summary>
/// Service layer responsible for business logic related to reservations.
/// Coordinates repositories, unit-of-work and mapping concerns.
/// </summary>
public class ReservationService : IReservationService
{
    /// <summary>
    /// Repository used to query and persist reservations.
    /// </summary>
    private readonly IReservationRepository _reservationRepository;

    /// <summary>
    /// Repository used to query tables.
    /// </summary>
    private readonly ITableRepository _tableRepository;

    /// <summary>
    /// Repository used to query restaurants.
    /// </summary>
    private readonly IRestaurantRepository _restaurantRepository;

    /// <summary>
    /// Unit of work used to coordinate transactions across repositories.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// AutoMapper instance for mapping between entities and DTOs.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Logger used for service-level diagnostics.
    /// </summary>
    private readonly ILogger<ReservationService> _logger;

    /// <summary>
    /// Create a new instance of <see cref="ReservationService"/>.
    /// </summary>
    /// <param name="reservationRepository">Reservation repository.</param>
    /// <param name="tableRepository">Table repository.</param>
    /// <param name="restaurantRepository">Restaurant repository.</param>
    /// <param name="unitOfWork">Unit of Work for transactions.</param>
    /// <param name="mapper">AutoMapper instance.</param>
    /// <param name="logger">Logger for this service.</param>
    public ReservationService(
        IReservationRepository reservationRepository,
        ITableRepository tableRepository,
        IRestaurantRepository restaurantRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ReservationService> logger)
    {
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
        _restaurantRepository = restaurantRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Create a new reservation for the specified user.
    /// Begins a transaction, validates availability and commits on success.
    /// </summary>
    public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto reservationDto, string userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(reservationDto.RestaurantId);
            if (restaurant == null)
                throw new ArgumentException("Restaurant not found");

            var timeOfDay = reservationDto.ReservationDate.TimeOfDay;
            if (timeOfDay < restaurant.OpeningTime || timeOfDay > restaurant.ClosingTime)
                throw new InvalidOperationException("Restaurant is closed at the requested time");

            var availableTable = await FindAvailableTableAsync(reservationDto);
            if (availableTable == null)
                throw new InvalidOperationException("No available tables for the requested time and party size");

            var reservation = new Reservation
            {
                ReservationDate = reservationDto.ReservationDate,
                NumberOfGuests = reservationDto.NumberOfGuests,
                SpecialRequests = reservationDto.SpecialRequests,
                UserId = userId,
                TableId = availableTable.Id,
                Status = Domain.Enums.ReservationStatus.Confirmed,
                ReservationTime = DateTime.UtcNow
            };

            var created = await _reservationRepository.AddAsync(reservation);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Reservation {ReservationId} created for user {UserId}", created.Id, userId);

            var res = await _reservationRepository.GetByIdAsync(created.Id);
            return _mapper.Map<ReservationDto>(res);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Find an available table for the requested reservation date and party size.
    /// </summary>
    private async Task<Table?> FindAvailableTableAsync(CreateReservationDto reservationDto)
    {
        var allReservations = await _reservationRepository.GetByRestaurantIdAsync(reservationDto.RestaurantId);

        var conflictingTableIds = allReservations
            .Where(r => r.Status != Domain.Enums.ReservationStatus.Cancelled &&
                        r.ReservationDate.Date == reservationDto.ReservationDate.Date &&
                        Math.Abs((r.ReservationDate - reservationDto.ReservationDate).TotalHours) < 2)
            .Select(r => r.TableId)
            .ToHashSet();

        var tables = await _tableRepository.GetByRestaurantIdAsync(reservationDto.RestaurantId);

        return tables
            .Where(t => t.Capacity >= reservationDto.NumberOfGuests && !conflictingTableIds.Contains(t.Id))
            .OrderBy(t => t.Capacity)
            .FirstOrDefault();
    }

    /// <summary>
    /// Get reservations for a user.
    /// </summary>
    public async Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(string userId)
    {
        var reservations = await _reservationRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    /// <summary>
    /// Get a reservation by id if it belongs to the provided user.
    /// </summary>
    public async Task<ReservationDto?> GetReservationByIdAsync(int reservationId, string userId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null || reservation.UserId != userId)
            return null;

        return _mapper.Map<ReservationDto>(reservation);
    }

    /// <summary>
    /// Cancel a reservation for the given user.
    /// </summary>
    public async Task CancelReservationAsync(int id, string userId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null || reservation.UserId != userId)
            throw new ArgumentException("Reservation not found");

        if (!reservation.CanBeModified())
            throw new InvalidOperationException("Cannot cancel this reservation");

        reservation.Cancel();
        await _reservationRepository.UpdateAsync(reservation);
        _logger.LogInformation("Reservation {ReservationId} cancelled by user {UserId}", id, userId);
    }

    /// <summary>
    /// Check table availability for the requested date, party size and restaurant.
    /// </summary>
    public async Task<IEnumerable<TableDto>> CheckAvailabilityAsync(ReservationAvailabilityDto availabilityDto)
    {
        var allReservations = await _reservationRepository.GetByRestaurantIdAsync(availabilityDto.RestaurantId);

        var conflictingTableIds = allReservations
            .Where(r => r.Status != Domain.Enums.ReservationStatus.Cancelled &&
                        r.ReservationDate.Date == availabilityDto.Date.Date &&
                        Math.Abs((r.ReservationDate - availabilityDto.Date).TotalHours) < 2)
            .Select(r => r.TableId)
            .ToHashSet();

        var tables = await _tableRepository.GetByRestaurantIdAsync(availabilityDto.RestaurantId);

        var available = tables
            .Where(t => t.Capacity >= availabilityDto.NumberOfGuests && !conflictingTableIds.Contains(t.Id))
            .ToList();

        return _mapper.Map<IEnumerable<TableDto>>(available);
    }
}
