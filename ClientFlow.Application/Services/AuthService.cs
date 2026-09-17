using ClientFlow.Application.DTOs.Auth;
using ClientFlow.Application.Exceptions;
using ClientFlow.Application.Interfaces;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Services
{
	public class AuthService
	{
		#region Members
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IJwtTokenService _jwtTokenService;
		#endregion Members

		#region Constructor
		public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
		{
			_userRepository  = userRepository;
			_passwordHasher  = passwordHasher;
			_jwtTokenService = jwtTokenService;
		}
		#endregion Constructor

		#region Public Methods
		public async Task<User> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
		{
			var email = request.Email.Trim().ToLowerInvariant();

			var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);

			if (existingUser is not null)
				throw new ConflictException("A user with this email already exists.");

			var passwordHash = _passwordHasher.Hash(request.Password);

			var user = new User(email, passwordHash, "User");

			await _userRepository.AddAsync(user, cancellationToken);

			return user;
		}

		public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

			if (user == null)
				throw new InvalidOperationException("Invalid email.");

			if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
				throw new UnauthorizedAccessException("Invalid password.");

			return new LoginResponse
			{
				AccessToken = _jwtTokenService.CreateToken(user),
				ExpiresAt   = DateTimeOffset.UtcNow.AddMinutes(5),
			};
		}
		#endregion Public Methods
	}
}
