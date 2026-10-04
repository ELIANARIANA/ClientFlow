using ClientFlow.Application.Interfaces;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Users
{
	public class UserService : IUserService
	{
		#region Members
		private readonly IUserRepository _userRepository;
		#endregion Members

		#region Constructor
		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		#endregion Constructor

		#region Public Methods
		public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
		{
			return await _userRepository.GetByEmailAsync(email, cancellationToken);
		}

		public async Task AddUserAsync(User user, CancellationToken cancellationToken)
		{
			await _userRepository.AddAsync(user, cancellationToken);
		}

		public async Task<List<User>> GetUsersAsync()
		{
			return await _userRepository.GetAsync();
		}

		public async Task<User?> UpdateUserAsync(Guid id, User user)
		{
			return await _userRepository.UpdateAsync(id, user);
		}

		public async Task<User?> DeleteUserAsync(Guid id)
		{
			return await _userRepository.DeleteAsync(id);
		}
		#endregion Public Methods
	}
}
