using Microsoft.AspNetCore.Mvc;

using ClientFlow.Application.Customers;
using ClientFlow.Domain.Entities;

namespace ClientFlow.Api.Controllers
{
	[ApiController]
	[Route("api/customers")]
	public class CustomersController : Controller
	{
		#region Members
		private readonly ILogger<CustomersController> _logger;
		private readonly ICustomerService _customerService;
		#endregion Members

		#region Constructor
		public CustomersController(ILogger<CustomersController> logger, ICustomerService customerService)
		{
			_logger = logger;
			_customerService = customerService;
		}
		#endregion Constructor

		#region Methods
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCustomerById(Guid id)
		{
			_logger.LogInformation($"Getting customer with ID: '{id}'...");

			var customer = await _customerService.GetCustomerByIdAsync(id);
			
			if (customer == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				return NotFound();
			}

			_logger.LogInformation($"Customer with ID: {id} retrieved successfully.");

			return Ok(customer);
		}

		[HttpGet]
		public async Task<IActionResult> GetCustomers()
		{
			_logger.LogInformation("Getting customers...");

			var customers = await _customerService.GetCustomersAsync();

			_logger.LogInformation($"Retrieved {customers.Count()} customers.");

			return Ok(customers);
		}

		[HttpPost]
		public async Task<IActionResult> AddCustomer([FromBody]Customer customer)
		{
			if (!IsValidCustomer(customer))
			{
				_logger.LogWarning("Invalid customer data received!");

				return BadRequest("Invalid customer data.");
			}
				

			_logger.LogInformation("Adding new customer...");

			await _customerService.AddCustomerAsync(customer);

			_logger.LogInformation($"Customer with ID: {customer.Id} created successfully.");

			return Ok(new { Message = "Customer created successfully.", Customer = customer });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomer(Guid id,Customer customer)
		{
			if (!IsValidCustomer(customer))
			{
				_logger.LogWarning("Invalid customer data received!");

				return BadRequest("Invalid customer data.");
			}
				
			_logger.LogInformation($"Updating customer with ID: '{id}'...");

			var result = await _customerService.UpdateCustomerAsync(id, customer);

			if (result == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				return NotFound();
			}
			
			_logger.LogInformation($"Customer with ID: {id} updated successfully.");

			return Ok(new { Message = "Customer updated successfully.", Customer = customer });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(Guid id)
		{
			_logger.LogInformation($"Deleting customer with ID: '{id}'...");

			var result = await _customerService.DeleteCustomerAsync(id);

			if (result == null)
			{
				_logger.LogWarning($"Customer with ID: {id} not found!");
				return NotFound();
			}

			_logger.LogInformation($"Customer with ID: {id} deleted successfully.");

			return Ok(new { Message = "Customer deleted successfully.", Customer = result });
		}
		#endregion Methods

		#region Helper Methods
		static bool IsValidCustomer(Customer customer)
		{
			try
			{
				if (customer == null                     ||
				string.IsNullOrEmpty(customer.FirstName) ||
				string.IsNullOrEmpty(customer.LastName ) ||
				string.IsNullOrEmpty(customer.Email    ))
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
