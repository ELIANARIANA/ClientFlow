using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerService 
	{
		Task<List<Customer>> GetCustomersAsync();
		Task<Customer> AddCustomerAsync(Customer customer);
	}
}
