using ClientFlow.Application.Interfaces;

namespace ClientFlow.Infrastructure.Security
{
	public class PasswordHasher : IPasswordHasher
	{
		public string Hash(string password)
		{
			// Implementation for hashing password
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public bool Verify(string password, string hash)
		{
			// Implementation for verifying password
			return BCrypt.Net.BCrypt.Verify(password, hash);
		}
	}
}
