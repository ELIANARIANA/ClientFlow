using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerRepository
	{
		Task<Customer?> GetByIdAsync(Guid id);
		Task<List<Customer>> GetAsync();
		Task<Customer> AddAsync(Customer customer);
		Task<Customer?> UpdateAsync(Guid id, Customer customer);
		Task<Customer?> DeleteAsync(Guid id);
	}
}
