using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerService 
	{
		Task<Customer?> GetCustomerByIdAsync(Guid id);
		Task<List<Customer>> GetCustomersAsync();
		Task<Customer> AddCustomerAsync(Customer customer);
	}
}
