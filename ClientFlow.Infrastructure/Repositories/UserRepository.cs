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

		public async Task<List<User>> GetAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<User?> UpdateAsync(Guid id, User user)
		{
			var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			
			if (existingUser == null)
				return null;

			_context.Entry(existingUser).CurrentValues.SetValues(user);
			
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<User?> DeleteAsync(Guid id)
		{
			var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

			if (existingUser == null)
				return await Task.FromResult<User?>(null);

			_context.Users.Remove(existingUser);
			await _context.SaveChangesAsync();

			return existingUser;
		}
		#endregion Public Methods
	}
}
