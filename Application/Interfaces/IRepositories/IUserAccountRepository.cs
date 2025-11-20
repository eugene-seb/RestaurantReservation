using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces.IRepositories;

public interface IUserAccountRepository
{
	Task<UserAccount?> GetByIdAsync(string id);
	Task<IEnumerable<UserAccount>> GetAllAsync();
	Task<UserAccount> AddAsync(UserAccount user);
	Task UpdateAsync(UserAccount user);
	Task DeleteAsync(string id);
}
