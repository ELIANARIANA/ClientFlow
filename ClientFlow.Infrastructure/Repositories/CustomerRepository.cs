using Microsoft.EntityFrameworkCore;

using ClientFlow.Application.Customers;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		#region Members
		private readonly AppDbContext _context;
		#endregion Members

		#region Constructor
		public CustomerRepository(AppDbContext context)
		{
			_context = context;
		}
		#endregion Constructor
		
		#region Methods
		public async Task<List<Customer>> GetAsync()
		{
			return await _context.Customers.ToListAsync();
		}

		public async Task<Customer> AddAsync(Customer customer)
		{
			await _context.Customers.AddAsync(customer);
			await _context.SaveChangesAsync();
			return customer;
		}
		#endregion Methods
	}
}
