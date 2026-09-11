using System.ComponentModel.DataAnnotations;


namespace ClientFlow.Domain.Entities
{
	public class User
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		[EmailAddress]
		public string Email { get; private set; } = string.Empty;
		public string PasswordHash { get; private set; } = string.Empty;
		public string Role { get; private set; } = "User";
		public DateTimeOffset CreatedAt { get; private set; }
		public DateTimeOffset? UpdatedAt { get; set; }

		private User() { }

		public User(string email, string passwordHash, string role)
		{
			Email        = email;
			PasswordHash = passwordHash;
			Role         = role;
			CreatedAt    = DateTimeOffset.UtcNow;
		}

		public void UpdatePassword(string passwordHash)
		{
			PasswordHash = passwordHash;
			UpdatedAt    = DateTimeOffset.UtcNow;
		}
	}
}
