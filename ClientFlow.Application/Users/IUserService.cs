using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Users
{
	public interface IUserService
	{
		Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
		Task AddUserAsync(User user, CancellationToken cancellationToken);
		Task<List<User>> GetUsersAsync();
		Task<User?> UpdateUserAsync(Guid id, User user);
		Task<User?> DeleteUserAsync(Guid id);
	}
}
