using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
		Task AddAsync(User user, CancellationToken cancellationToken);
		Task<List<User>> GetAsync();
		Task<User?> UpdateAsync(Guid id, User user);
		Task<User?> DeleteAsync(Guid id);

	}
}
