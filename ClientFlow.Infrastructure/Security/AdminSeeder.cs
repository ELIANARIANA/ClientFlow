using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

using ClientFlow.Application.Interfaces;
using ClientFlow.Domain.Entities;
using ClientFlow.Domain.Enums;

namespace ClientFlow.Infrastructure.Security
{
	public class AdminSeeder
	{
		#region Members
		private readonly ILogger<AdminSeeder> _logger;
		private readonly AppDbContext _context;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IConfiguration _configuration;
		#endregion Members

		#region Constructor
		public AdminSeeder(ILogger<AdminSeeder> logger, AppDbContext context, IPasswordHasher passwordHasher, IConfiguration configuration)
		{
			_logger         = logger;
			_context        = context;
			_passwordHasher = passwordHasher;
			_configuration  = configuration;
		}
		#endregion Constructor

		public async Task SeederAsync(CancellationToken cancellationToken = default)
		{
			_logger.LogInformation("Checking for existing admin user.");

			var adminEmail    = _configuration["Admin:Email"];
			var adminPassword = _configuration["Admin:Password"];

			var existingAdmin = await _context.Users
			.FirstOrDefaultAsync(x => x.Email == adminEmail, cancellationToken);

			if (existingAdmin != null)
			{
				_logger.LogInformation("Admin user already exists");
				return;
			}

			if (string.IsNullOrEmpty(adminEmail))
				throw new ValidationException("admin email is not configured.");

			if (string.IsNullOrEmpty(adminPassword))
				throw new ValidationException("admin password is not configured.");

			var passwordHash = _passwordHasher.Hash(adminPassword);

			await _context.Users.AddAsync(new User(adminEmail, passwordHash, UserRole.Admin), cancellationToken);

			await _context.SaveChangesAsync(cancellationToken);

			_logger.LogInformation("Adding and saving admin user successfully.");
		}
		
	}
}
