using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

			var problemDetails = new ProblemDetails
			{
				Status   = StatusCodes.Status500InternalServerError,
				Title    = "An unexpected error occurred.",
				Detail   = exception.Message,
				Instance = httpContext.Request.Path
			};

			problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

			httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
		#endregion Methods
	}
}
