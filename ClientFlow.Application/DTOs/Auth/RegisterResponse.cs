namespace ClientFlow.Application.DTOs.Auth
{
	public class RegisterResponse
	{
		public Guid Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public DateTimeOffset CreatedAt { get; set; }
	}
}
