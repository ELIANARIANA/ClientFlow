using System.ComponentModel.DataAnnotations;

namespace ClientFlow.Application.DTOs.Auth
{
	public class LoginRequest
	{
		[EmailAddress]
		public string Email    { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
