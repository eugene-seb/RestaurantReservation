using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces.IRepositories;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(string id);
	Task<IEnumerable<User>> GetAllAsync();
	Task<User?> GetByEmailAsync(string email);
	Task<User> AddAsync(User user);
	Task UpdateAsync(User user);
	Task DeleteAsync(string id);
}
