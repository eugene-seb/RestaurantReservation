using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces.IRepositories;

public interface IReservationRepository
{
	Task<Reservation?> GetByIdAsync(int id);
	Task<IEnumerable<Reservation>> GetAllAsync();
	Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId);
	Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(int restaurantId);
	Task<Reservation> AddAsync(Reservation reservation);
	Task UpdateAsync(Reservation reservation);
	Task DeleteAsync(int id);
}
