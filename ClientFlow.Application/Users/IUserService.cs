using ClientFlow.Application.DTOs.User;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Users
{
	public interface IUserService
	{
		Task<GetUsersResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
		Task<AddUserResponse> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken);
		Task<List<GetUsersResponse>> GetUsersAsync();
		Task<User?> UpdateUserAsync(Guid id, UpdateUserRequest request);
		Task<User?> DeleteUserAsync(Guid id);
	}
}
