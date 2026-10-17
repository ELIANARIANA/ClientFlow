using FluentValidation;

using ClientFlow.Application.DTOs.User;
using ClientFlow.Application.Exceptions;
using ClientFlow.Application.Interfaces;
using ClientFlow.Domain.Entities;
using ClientFlow.Domain.Enums;

namespace ClientFlow.Application.Users
{
	public class UserService : IUserService
	{
		#region Members
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IValidator<AddUserRequest> _addUserValidator;
		private readonly IValidator<UpdateUserRequest> _updateUserValidator;
		#endregion Members

		#region Constructor
		public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IValidator<AddUserRequest> addUserValidator,
		IValidator<UpdateUserRequest> updateUserValidator)
		{
			_userRepository = userRepository;
			_passwordHasher   = passwordHasher;
			_addUserValidator = addUserValidator;
			_updateUserValidator = updateUserValidator;
		}
		#endregion Constructor

		#region Public Methods
		public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
		{
			return await _userRepository.GetByEmailAsync(email, cancellationToken);
		}

		public async Task<AddUserResponse> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken)
		{
			var validationResult =
			await _addUserValidator.ValidateAsync(
				request,
				cancellationToken);

			if (!validationResult.IsValid)
			{
				var errors = validationResult
					.Errors
					.GroupBy(x => x.PropertyName)
					.ToDictionary(
						x => x.Key,
						x => x.Select(e => e.ErrorMessage).ToArray());

				throw new Exceptions.ValidationException(errors);
			}

			var email = request.Email.Trim().ToLowerInvariant();

			var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);

			if (existingUser is not null)
				throw new ConflictException("A user with this email already exists.");

			var passwordHash = _passwordHasher.Hash(request.Password);

			var user = new User(email, passwordHash, (UserRole)request.Role);

			await _userRepository.AddAsync(user, cancellationToken);

			return new AddUserResponse
			{
				Id        = user.Id,
				Email     = user.Email,
				Role      = user.Role,
				CreatedAt = user.CreatedAt
			};
		}

		public async Task<List<GetUsersResponse>> GetUsersAsync()
		{
			var users = await _userRepository.GetAsync();
			
			return users.Select(u => new GetUsersResponse
			{
				Email     = u.Email,
				Role      = u.Role,
				CreatedAt = u.CreatedAt,
				UpdatedAt = u.UpdatedAt
			}).ToList();
		}

		public async Task<User?> UpdateUserAsync(Guid id, UpdateUserRequest request)
		{
			var validationResult = await _updateUserValidator.ValidateAsync(request);

			if (!validationResult.IsValid)
			{
				var errors = validationResult
					.Errors
					.GroupBy(x => x.PropertyName)
					.ToDictionary(
						x => x.Key,
						x => x.Select(e => e.ErrorMessage).ToArray());

				throw new Exceptions.ValidationException(errors);
			}

			var passwordHash = _passwordHasher.Hash(request.Password);

			var user = new User(request.Email, passwordHash, (UserRole)request.Role);

			return await _userRepository.UpdateAsync(id, user);
		}

		public async Task<User?> DeleteUserAsync(Guid id)
		{
			return await _userRepository.DeleteAsync(id);
		}
		#endregion Public Methods
	}
}
