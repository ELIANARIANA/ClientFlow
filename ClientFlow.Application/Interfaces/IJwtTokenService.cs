using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Interfaces
{
	public interface IJwtTokenService
	{
		string CreateToken(User user);
	}
}
