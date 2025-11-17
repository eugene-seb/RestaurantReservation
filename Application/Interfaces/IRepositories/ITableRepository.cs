using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces.IRepositories;

public interface ITableRepository
{
	Task<Table?> GetByIdAsync(int id);
	Task<IEnumerable<Table>> GetAllAsync();
	Task<IEnumerable<Table>> GetByRestaurantIdAsync(int restaurantId);
	Task<Table> AddAsync(Table table);
	Task UpdateAsync(Table table);
	Task DeleteAsync(int id);
}
