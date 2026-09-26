using ClientFlow.Domain.Enums;

namespace ClientFlow.Application.DTOs.Auth
{
	public class RegisterResponse
	{
		public Guid Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public int Role { get; set; } = (int)UserRole.None;
		public DateTimeOffset CreatedAt { get; set; }
	}
}
