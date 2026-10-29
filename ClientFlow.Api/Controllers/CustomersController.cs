using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ClientFlow.Application.Customers;
using ClientFlow.Application.DTOs.Customer;
using ClientFlow.Application.Exceptions;
using ClientFlow.Domain.Enums;

namespace ClientFlow.Api.Controllers
{
	/// <summary>
	/// Controller for managing customers.
	/// </summary>
	[Authorize]
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
			_logger          = logger;
			_customerService = customerService;
		}
		#endregion Constructor

		#region Methods
		/// <summary>
		/// Gets a customer by their ID.
		/// </summary>
		/// <param name="id"></param>
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

		/// <summary> Gets customers.</summary>
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
		/// <param name="request"></param>
		[Authorize(Roles = nameof(UserRole.Admin))]
		[HttpPost]
		public async Task<IActionResult> AddCustomer([FromBody]CustomerRequest request)
		{
			_logger.LogInformation("Adding new customer...");

			var customer = await _customerService.AddCustomerAsync(request);

			_logger.LogInformation($"Customer with ID: {customer.Id} created successfully.");

			return Ok(new { Message = "Customer created successfully.", Customer = customer });
		}

		/// <summary>
		/// Updates an existing customer.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="request"></param>
		[Authorize(Roles = nameof(UserRole.Admin))]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomer(Guid id, CustomerRequest request)
		{
			_logger.LogInformation($"Updating customer with ID: '{id}'...");

			var result = await _customerService.UpdateCustomerAsync(id, request);

			if (result == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				throw new NotFoundException($"Customer with ID: {id} not found.");
			}
			
			_logger.LogInformation($"Customer with ID: {id} updated successfully.");

			return Ok(new { Message = "Customer updated successfully.", Customer = result });
		}

		/// <summary>
		/// Deletes a customer by their ID.
		/// </summary>
		/// <param name="id"></param>
		[Authorize(Roles = nameof(UserRole.Admin))]
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
	}
}
