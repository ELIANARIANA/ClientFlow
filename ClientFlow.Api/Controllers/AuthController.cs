using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ClientFlow.Application.DTOs.Auth;
using ClientFlow.Application.Services;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Api.Controllers
{
	/// <summary>
	/// Controller for Authorization.
	/// </summary>
	[ApiController]
	[Route("api/Auth")]
	public class AuthController : ControllerBase
	{
		#region Members
		private readonly ILogger<AuthController> _logger;
		private readonly AuthService _authService;
		#endregion Members

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthController"/> class.
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="authService"></param>
		public AuthController(ILogger<AuthController> logger, AuthService authService)
		{
			_logger      = logger;
			_authService = authService;
		}
		#endregion Constructor

		#region Public Methods
		/// <summary>
		/// User register.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<ActionResult<User>> Register(RegisterRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.RegisterAsync(request, cancellationToken);

			return Ok(response);
		}

		/// <summary>
		/// user login.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.LoginAsync(request, cancellationToken);

			return Ok(response);
		}
		#endregion Public Methods
	}
}
