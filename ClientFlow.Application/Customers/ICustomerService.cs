using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerService 
	{
		Task<Customer> AddCustomerAsync(Customer customer);
	}
}
