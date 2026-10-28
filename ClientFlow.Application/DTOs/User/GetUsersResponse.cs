namespace ClientFlow.Application.DTOs.User
{
	public class GetUsersResponse
	{
		public Guid UserId { get; set; }
		public string Email { get; set; }
		public int Role { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset? UpdatedAt { get; set; }
	}
}
