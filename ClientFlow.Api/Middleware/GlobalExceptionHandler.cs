using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using ClientFlow.Application.Exceptions;

namespace ClientFlow.Api.Middleware
{
	/// <summary>
	/// Global exception handler that logs unhandled exceptions and returns a standardized error response.
	/// </summary>
	public class GlobalExceptionHandler : IExceptionHandler
	{
		#region Members
		private readonly ILogger<GlobalExceptionHandler> _logger;
		#endregion Members

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
		/// </summary>
		/// <param name="logger"></param>
		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			_logger = logger;
		}
		#endregion Constructor

		#region Methods
		/// <summary>
		/// Tries to handle an unhandled exception by logging it and returning a standardized error response.
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="exception"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			_logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

			var statusCode = exception switch
			{
				ArgumentException     => StatusCodes.Status400BadRequest,
				ValidationException   => StatusCodes.Status400BadRequest,
				NotFoundException     => StatusCodes.Status404NotFound,
				UnauthorizedException => StatusCodes.Status401Unauthorized,
				ForbiddenException    => StatusCodes.Status403Forbidden,
				ConflictException     => StatusCodes.Status409Conflict,
				_ => StatusCodes.Status500InternalServerError
			};

			var title = exception switch
			{
				ArgumentException     => "Invalid argument provided.",
				ValidationException   => "Validation failed for the request.",
				NotFoundException     => "The requested resource was not found.",
				UnauthorizedException => "You are not authorized to access this resource.",
				ForbiddenException    => "You do not have permission to access this resource.",
				ConflictException     => "A conflict occurred with the current state of the resource.",
				_ => "An unexpected error occurred."
			};

			var detail = statusCode == StatusCodes.Status500InternalServerError
				? "An unexpected error occurred while processing your request."
				: exception.Message;


			var problemDetails = new ProblemDetails
			{
				Status   = statusCode,
				Title    = title,
				Detail   = detail,
				Instance = httpContext.Request.Path
			};

			problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

			httpContext.Response.StatusCode = statusCode;

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
		#endregion Methods
	}
}
