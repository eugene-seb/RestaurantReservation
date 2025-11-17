using AutoMapper;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Services;

/// <summary>
/// Service responsible for restaurant-related business operations.
/// Handles creation, update, deletion and reads via the repository and unit-of-work.
/// </summary>
public class RestaurantService : IRestaurantService
{
    /// <summary>
    /// Repository used to perform restaurant persistence operations.
    /// </summary>
    private readonly IRestaurantRepository _repository;

    /// <summary>
    /// Unit of Work used to coordinate transactions across repositories.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// AutoMapper instance for mapping between entities and DTOs.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Logger used for service-level diagnostics.
    /// </summary>
    private readonly ILogger<RestaurantService> _logger;

    /// <summary>
    /// Construct a new <see cref="RestaurantService"/> instance.
    /// </summary>
    /// <param name="repository">Repository for restaurant data.</param>
    /// <param name="unitOfWork">Unit of Work for transactions.</param>
    /// <param name="mapper">AutoMapper instance.</param>
    /// <param name="logger">Logger for this service.</param>
    public RestaurantService(IRestaurantRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<RestaurantService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve a restaurant by its identifier and map to DTO.
    /// </summary>
    public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = await _repository.GetByIdAsync(id);
        if (restaurant == null) return null;
        return _mapper.Map<RestaurantDto>(restaurant);
    }

    /// <summary>
    /// Retrieve all restaurants and map to DTOs.
    /// </summary>
    public async Task<IEnumerable<RestaurantDto>> GetRestaurantsAsync()
    {
        var restaurants = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    /// <summary>
    /// Search restaurants by name and map to DTOs.
    /// </summary>
    public async Task<IEnumerable<RestaurantDto>> GetRestaurantsByNameAsync(string restaurantName)
    {
        var restaurants = await _repository.SearchByNameAsync(restaurantName);
        return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    /// <summary>
    /// Create a new restaurant from the provided DTO.
    /// </summary>
    public async Task<Restaurant> CreateRestaurantAsync(CreateRestaurantDto restaurantDto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = _mapper.Map<Restaurant>(restaurantDto);
            var created = await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Restaurant {RestaurantId} created", created.Id);
            return created;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Update an existing restaurant identified by <paramref name="restaurantId"/>.
    /// </summary>
    public async Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto)
    {
        var existing = await _repository.GetByIdAsync(restaurantId);
        if (existing == null)
            throw new ArgumentException("Restaurant not found");

        _mapper.Map(restaurantDto, existing);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _repository.UpdateAsync(existing);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Restaurant {RestaurantId} updated", restaurantId);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Delete a restaurant by identifier.
    /// </summary>
    public async Task DeleteRestaurantAsync(int restaurantId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _repository.DeleteAsync(restaurantId);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Restaurant {RestaurantId} deleted", restaurantId);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
