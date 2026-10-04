using ClientFlow.Application.Exceptions;
using ClientFlow.Application.Users;
using ClientFlow.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientFlow.Api.Controllers
{
	/// <summary>
	/// Controller for managing users.
	/// </summary>
	//[Authorize]
	[ApiController]
	[Route("api/users")]
	public class UsersController : Controller
	{
		#region Members
		private readonly ILogger<UsersController> _logger;
		private readonly IUserService _userService;
		#endregion Members

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController"/> class.
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="userService"></param>
		public UsersController(ILogger<UsersController> logger, IUserService userService)
		{
			_logger      = logger;
			_userService = userService;
		}
		#endregion Constructor

		#region Public Methods
		/// <summary>
		/// Adds a new user.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> AddUser([FromBody]User user)
		{
			_logger.LogInformation("Adding new user...");

			await _userService.AddUserAsync(user, CancellationToken.None);

			_logger.LogInformation($"User with ID: {user.Id} created successfully.");

			return Ok(new { Message = "User created successfully.", User = user });
		}

		/// <summary>
		/// Gets a list of all users.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			_logger.LogInformation("Getting users...");

			var users = await _userService.GetUsersAsync();

			_logger.LogInformation($"Retrieved {users.Count()} Users.");

			return Ok(users);
		}

		/// <summary>
		/// Updates a user by their ID.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(Guid id, [FromBody]User user)
		{
			_logger.LogInformation("Updating user...");

			await _userService.UpdateUserAsync(id, user);
			
			_logger.LogInformation($"User with ID: {id} updated successfully.");
			
			return Ok(new { Message = "User updated successfully." });
		}

		/// <summary>
		/// Deletes a user by their ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(Guid id)
		{
			_logger.LogInformation($"Deleting user with ID: '{id}'...");

			var result = await _userService.DeleteUserAsync(id);

			if (result == null)
			{
				_logger.LogWarning($"User with ID: {id} not found!");
				throw new NotFoundException($"User with ID: {id} not found.");
			}

			_logger.LogInformation($"User with ID: {id} deleted successfully.");

			return Ok(new { Message = "User deleted successfully.", User = result });
		}
		#endregion Public Methods
	}
}
