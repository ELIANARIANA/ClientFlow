using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerRepository
	{
		Task<Customer> AddAsync(Customer customer);
	}
}
