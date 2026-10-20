using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ClientFlow.Application.Exceptions;
using ClientFlow.Application.Users;
using ClientFlow.Application.DTOs.User;
using ClientFlow.Domain.Enums;

namespace ClientFlow.Api.Controllers
{
	/// <summary>
	/// Controller for managing users.
	/// </summary>
	[Authorize]
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
		/// <param name="request"></param>
		/// <returns></returns>
		[Authorize(Roles = nameof(UserRole.Admin))]
		[HttpPost]
		public async Task<ActionResult<AddUserResponse>> AddUser([FromBody]AddUserRequest request)
		{
			_logger.LogInformation("Adding new user...");

			var response = await _userService.AddUserAsync(request, CancellationToken.None);

			_logger.LogInformation($"User with ID: {response.Id} created successfully.");

			return Ok(new { Message = "User created successfully.", AddUserResponse = response });
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
		/// <param name="request"></param>
		/// <returns></returns>
		[Authorize(Roles = nameof(UserRole.Admin))]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UpdateUserRequest request)
		{
			_logger.LogInformation($"Updating user with ID: {id}...");

			var result = await _userService.UpdateUserAsync(id, request);

			if (result == null)
			{
				_logger.LogWarning($"User with ID: {id} not found!");
				throw new NotFoundException($"User with ID: {id} not found.");
			}

			_logger.LogInformation($"User with ID: {id} updated successfully.");
			
			return Ok(new { Message = "User updated successfully." });
		}

		/// <summary>
		/// Deletes a user by their ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		[Authorize(Roles = nameof(UserRole.Admin))]
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
