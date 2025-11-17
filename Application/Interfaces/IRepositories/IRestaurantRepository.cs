using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces.IRepositories;

public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(int id);
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant> AddAsync(Restaurant restaurant);
    Task UpdateAsync(Restaurant restaurant);
    Task DeleteAsync(int id);
    Task<IEnumerable<Restaurant>> SearchByNameAsync(string name);
}
