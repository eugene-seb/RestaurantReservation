using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces.IRepositories;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int id);
    Task<IEnumerable<Address>> GetAllAsync();
    Task<IEnumerable<Address>> GetByRestaurantIdAsync(int restaurantId);
    Task<Address> AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(int id);
}
