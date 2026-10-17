namespace ClientFlow.Application.DTOs.User
{
	public class GetUsersResponse
	{
		public string Email { get; set; }
		public int Role { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset? UpdatedAt { get; set; }
	}
}
