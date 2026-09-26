using System.ComponentModel.DataAnnotations;

using ClientFlow.Domain.Enums;

namespace ClientFlow.Domain.Entities
{
	public class User
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		[EmailAddress]
		public string Email { get; private set; } = string.Empty;
		public string PasswordHash { get; private set; } = string.Empty;
		public int Role { get; private set; } = (int)UserRole.User;
		public DateTimeOffset CreatedAt { get; private set; }
		public DateTimeOffset? UpdatedAt { get; set; }

		private User() { }

		public User(string email, string passwordHash, UserRole role)
		{
			Email        = email;
			PasswordHash = passwordHash;
			Role         = (int)role;
			CreatedAt    = DateTimeOffset.UtcNow;
		}

		public void UpdatePassword(string passwordHash)
		{
			PasswordHash = passwordHash;
			UpdatedAt    = DateTimeOffset.UtcNow;
		}
	}
}
