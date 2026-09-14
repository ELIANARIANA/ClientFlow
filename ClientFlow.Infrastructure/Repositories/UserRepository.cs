using ClientFlow.Application.Interfaces;
using ClientFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientFlow.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		#region Members
		private readonly AppDbContext _context;
		#endregion Members

		#region Constructor
		public UserRepository(AppDbContext context)
		{
			_context = context;
		}
		#endregion Constructor

		#region Public Methods
		public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
		{
			return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
		}

		public async Task AddAsync(User user, CancellationToken cancellationToken = default)
		{
			await _context.Users.AddAsync(user, cancellationToken);

			await _context.SaveChangesAsync(cancellationToken);
		}
		#endregion Public Methods
	}
}
