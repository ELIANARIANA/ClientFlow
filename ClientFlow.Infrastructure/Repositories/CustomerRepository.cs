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
		public async Task<Customer?> GetByIdAsync(Guid id)
		{
			return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
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

		public async Task<Customer?> UpdateAsync(Guid id, Customer customer)
		{
			var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

			if (existingCustomer == null)
				return null;

			_context.Entry(existingCustomer).CurrentValues.SetValues(customer);

			await _context.SaveChangesAsync();
			return customer;
		}

		public async Task<Customer?> DeleteAsync(Guid id)
		{
			var existingCustomer = _context.Customers.FirstOrDefault(x => x.Id == id);

			if (existingCustomer == null)
				return await Task.FromResult<Customer?>(null);

			_context.Customers.Remove(existingCustomer);
			await _context.SaveChangesAsync();

			return existingCustomer;
		}
		#endregion Methods
	}
}
