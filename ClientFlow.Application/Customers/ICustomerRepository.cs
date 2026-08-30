using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public interface ICustomerRepository
	{
		Task<List<Customer>> GetAsync();
		Task<Customer> AddAsync(Customer customer);
	}
}
