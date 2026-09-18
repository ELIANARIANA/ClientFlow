using System.ComponentModel.DataAnnotations;

namespace ClientFlow.Application.DTOs.Auth
{
	public class RegisterRequest
	{
		[EmailAddress]
		public string Email    { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
