using System.ComponentModel.DataAnnotations;

namespace ClientFlow.Application.DTOs.User
{
	public class UpdateUserRequest
	{
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public int Role { get; set; } = 0;
	}
}
