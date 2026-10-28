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
		
		#region Public Methods
		public async Task<Customer?> GetByIdAsync(Guid customerId)
		{
			return await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerId);
		}

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

		public async Task<Customer?> UpdateAsync(Guid customerId, Customer customer)
		{
			var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerId);

			if (existingCustomer == null)
				return null;

			customer.CreatedAt = existingCustomer.CreatedAt;

			_context.Entry(existingCustomer).CurrentValues.SetValues(customer);

			await _context.SaveChangesAsync();
			return customer;
		}

		public async Task<Customer?> DeleteAsync(Guid customerId)
		{
			var existingCustomer = _context.Customers.FirstOrDefault(x => x.CustomerId == customerId);

			if (existingCustomer == null)
				return await Task.FromResult<Customer?>(null);

			_context.Customers.Remove(existingCustomer);
			await _context.SaveChangesAsync();

			return existingCustomer;
		}
		#endregion Public Methods
	}
}
