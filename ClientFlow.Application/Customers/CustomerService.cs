using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public class CustomerService : ICustomerService
	{
		#region Members
		private readonly ICustomerRepository _customerRepository;
		#endregion Members

		#region Constructor
		public CustomerService(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}
		#endregion Constructor

		#region Methods
		public async Task<Customer?> GetCustomerByIdAsync(Guid id)
		{
			return await _customerRepository.GetByIdAsync(id);
		}

		public async Task<List<Customer>> GetCustomersAsync()
		{
			return await _customerRepository.GetAsync();
		}

		public async Task<Customer> AddCustomerAsync(Customer customer)
		{
			return await _customerRepository.AddAsync(customer);
		}

		public async Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer)
		{
			return await _customerRepository.UpdateAsync(id, customer);
		}

		public async Task<Customer?> DeleteCustomerAsync(Guid id)
		{
			return await _customerRepository.DeleteAsync(id);
		}
		#endregion Methods
	}
}
