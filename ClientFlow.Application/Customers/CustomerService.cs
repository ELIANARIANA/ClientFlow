using FluentValidation;

using ClientFlow.Application.DTOs.Customer;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public class CustomerService : ICustomerService
	{
		#region Members
		private readonly ICustomerRepository _customerRepository;
		private readonly IValidator<AddCustomerRequest> _addCustomerValidator;
		#endregion Members

		#region Constructor
		public CustomerService(ICustomerRepository customerRepository, IValidator<AddCustomerRequest> addCustomerValidator)
		{
			_customerRepository = customerRepository;
			_addCustomerValidator = addCustomerValidator;
		}
		#endregion Constructor

		#region Public Methods
		public async Task<Customer?> GetCustomerByIdAsync(Guid id)
		{
			return await _customerRepository.GetByIdAsync(id);
		}

		public async Task<List<Customer>> GetCustomersAsync()
		{
			return await _customerRepository.GetAsync();
		}

		public async Task<Customer> AddCustomerAsync(AddCustomerRequest request)
		{
			var validationResult = await _addCustomerValidator.ValidateAsync(request);

			if(!validationResult.IsValid)
			{
				var errors = validationResult
					.Errors
					.GroupBy(x => x.PropertyName)
					.ToDictionary(
						x => x.Key,
						x => x.Select(e => e.ErrorMessage).ToArray());

				throw new Exceptions.ValidationException(errors);
			}

			return await _customerRepository.AddAsync(new Customer
			{
				Id          = Guid.NewGuid(),
				FirstName   = request.FirstName,
				LastName    = request.LastName,
				Email       = request.Email,
				Phone       = request.Phone,
				CompanyName = request.CompanyName,
				CreatedAt   = DateTimeOffset.UtcNow,
			});
		}

		public async Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer)
		{
			return await _customerRepository.UpdateAsync(id, customer);
		}

		public async Task<Customer?> DeleteCustomerAsync(Guid id)
		{
			return await _customerRepository.DeleteAsync(id);
		}
		#endregion Public Methods
	}
}
