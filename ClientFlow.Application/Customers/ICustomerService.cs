using ClientFlow.Application.DTOs.Customer;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerService 
	{
		Task<Customer?> GetCustomerByIdAsync(Guid id);
		Task<List<Customer>> GetCustomersAsync();
		Task<Customer> AddCustomerAsync(CustomerRequest request);
		Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer);
		Task<Customer?> DeleteCustomerAsync(Guid id);
	}
}
