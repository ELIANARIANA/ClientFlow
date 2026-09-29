using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using ClientFlow.Application.Interfaces;
using ClientFlow.Domain.Entities;
using ClientFlow.Domain.Enums;

namespace ClientFlow.Infrastructure.Security
{
	public class JwtTokenService : IJwtTokenService
	{
		#region Members
		private readonly IConfiguration _configuration;
		#endregion Members

		#region Constructor
		public JwtTokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		#endregion Constructor

		#region Public Methods
		public string CreateToken(User user)
		{
			var key = _configuration["Jwt:Key"]
				?? throw new InvalidOperationException(
				"Jwt Key is not configured");

			var issuer = _configuration["Jwt:Issuer"]
				?? throw new InvalidOperationException(
				"Jwt Issuer is not configured");

			var audience = _configuration["Jwt:Audience"]
				?? throw new InvalidOperationException(
				"Jwt Audience is not configured");

			var expirationMinutes = int.TryParse(
				_configuration["Jwt:ExpireMinutes"], out var minutes)
				? minutes
				: throw new InvalidOperationException(
				"Jwt ExpireMinutes is not configured");

			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new(JwtRegisteredClaimNames.Email, user.Email),
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Email, user.Email),
			};

			var roles = (UserRole)user.Role;

			if (roles.HasFlag(UserRole.User))
				claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.User)));

			if (roles.HasFlag(UserRole.Admin))
				claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.Admin)));

			if (roles.HasFlag(UserRole.Manager))
				claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.Manager)));

			var seurityKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(key));

			var credentials = new SigningCredentials(
				seurityKey,
				SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler()
				.WriteToken(token);
		}
		#endregion Public Methods
	}
}
