using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerService 
	{
		Task<Customer?> GetCustomerByIdAsync(Guid id);
		Task<List<Customer>> GetCustomersAsync();
		Task<Customer> AddCustomerAsync(Customer customer);
		Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer);
		Task<Customer?> DeleteCustomerAsync(Guid id);
	}
}
