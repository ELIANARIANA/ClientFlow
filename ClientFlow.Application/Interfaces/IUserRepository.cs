using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
		Task AddAsync(User user, CancellationToken cancellationToken);
	}
}
