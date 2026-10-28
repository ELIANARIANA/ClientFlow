using FluentValidation;

using ClientFlow.Application.DTOs.Customer;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Application.Customers
{
	public class CustomerService : ICustomerService
	{
		#region Members
		private readonly ICustomerRepository _customerRepository;
		private readonly IValidator<CustomerRequest> _customerValidator;
		#endregion Members

		#region Constructor
		public CustomerService(ICustomerRepository customerRepository, IValidator<CustomerRequest> customerValidator)
		{
			_customerRepository = customerRepository;
			_customerValidator  = customerValidator;
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

		public async Task<Customer> AddCustomerAsync(CustomerRequest request)
		{
			var validationResult = await _customerValidator.ValidateAsync(request);

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
				CustomerId  = Guid.NewGuid(),
				FirstName   = request.FirstName,
				LastName    = request.LastName,
				Email       = request.Email,
				Phone       = request.Phone,
				CompanyName = request.CompanyName,
				CreatedAt   = DateTimeOffset.UtcNow,
			});
		}

		public async Task<Customer?> UpdateCustomerAsync(Guid customerId, CustomerRequest request)
		{
			var validationResult = await _customerValidator.ValidateAsync(request);

			if (!validationResult.IsValid)
			{
				var errors = validationResult
					.Errors
					.GroupBy(x => x.PropertyName)
					.ToDictionary(
						x => x.Key,
						x => x.Select(e => e.ErrorMessage).ToArray());

				throw new Exceptions.ValidationException(errors);
			}

			return await _customerRepository.UpdateAsync(customerId, new Customer
			{
				CustomerId  = customerId,
				FirstName   = request.FirstName,
				LastName    = request.LastName,
				Email       = request.Email,
				Phone       = request.Phone,
				CompanyName = request.CompanyName,
				UpdatedAt   = DateTimeOffset.UtcNow,
			});
		}

		public async Task<Customer?> DeleteCustomerAsync(Guid id)
		{
			return await _customerRepository.DeleteAsync(id);
		}
		#endregion Public Methods
	}
}
