using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

using ClientFlow.Application.Customers;
using ClientFlow.Application.Exceptions;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Api.Controllers
{
	/// <summary>
	/// Controller for managing customers.
	/// </summary>
	[ApiController]
	[Route("api/customers")]
	public class CustomersController : Controller
	{
		#region Members
		private readonly ILogger<CustomersController> _logger;
		private readonly ICustomerService _customerService;
		#endregion Members

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomersController"/> class.
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="customerService"></param>
		public CustomersController(ILogger<CustomersController> logger, ICustomerService customerService)
		{
			_logger = logger;
			_customerService = customerService;
		}
		#endregion Constructor

		#region Methods
		/// <summary>
		/// Gets a customer by their ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCustomerById(Guid id)
		{
			_logger.LogInformation($"Getting customer with ID: '{id}'...");

			var customer = await _customerService.GetCustomerByIdAsync(id);
			
			if (customer == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				throw new NotFoundException($"Customer with ID: {id} not found.");
			}

			_logger.LogInformation($"Customer with ID: {id} retrieved successfully.");

			return Ok(customer);
		}

		/// <summary>
		/// Gets customers.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetCustomers()
		{
			_logger.LogInformation("Getting customers...");

			var customers = await _customerService.GetCustomersAsync();

			_logger.LogInformation($"Retrieved {customers.Count()} customers.");

			return Ok(customers);
		}

		/// <summary>
		/// Adds a new customer.
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> AddCustomer([FromBody]Customer customer)
		{
			_logger.LogInformation("Adding new customer...");

			if (!IsValidCustomer(customer))
			{
				_logger.LogWarning("Invalid customer data received!");
				throw new ValidationException("Invalid customer data received.");
			}

			await _customerService.AddCustomerAsync(customer);

			_logger.LogInformation($"Customer with ID: {customer.Id} created successfully.");

			return Ok(new { Message = "Customer created successfully.", Customer = customer });
		}

		/// <summary>
		/// Updates an existing customer.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="customer"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomer(Guid id,Customer customer)
		{
			if (!IsValidCustomer(customer))
			{
				_logger.LogWarning("Invalid customer data received!");
				throw new ValidationException("Invalid customer data.");
			}
				
			_logger.LogInformation($"Updating customer with ID: '{id}'...");

			var result = await _customerService.UpdateCustomerAsync(id, customer);

			if (result == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				throw new NotFoundException($"Customer with ID: {id} not found.");
			}
			
			_logger.LogInformation($"Customer with ID: {id} updated successfully.");

			return Ok(new { Message = "Customer updated successfully.", Customer = customer });
		}

		/// <summary>
		/// Deletes a customer by their ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(Guid id)
		{
			_logger.LogInformation($"Deleting customer with ID: '{id}'...");

			var result = await _customerService.DeleteCustomerAsync(id);

			if (result == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				throw new NotFoundException($"Customer with ID: {id} not found.");
			}

			_logger.LogInformation($"Customer with ID: {id} deleted successfully.");

			return Ok(new { Message = "Customer deleted successfully.", Customer = result });
		}
		#endregion Methods

		#region Helper Methods

		/// <summary>
		/// Validates the customer object to ensure it has required fields.
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>
		static bool IsValidCustomer(Customer customer)
		{
			try
			{
				if (customer == null                     ||
				string.IsNullOrEmpty(customer.FirstName) ||
				string.IsNullOrEmpty(customer.LastName ) ||
				string.IsNullOrEmpty(customer.Email    ) &&
				new MailAddress(customer.Email).Address
				== customer.Email)
				{
					return false;
				}

				return true;
			}
			catch
			{
				return false;
			}
		}
		#endregion Helper Methods
	}
}
