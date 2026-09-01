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
		private readonly ICustomerService _customerService;
		#endregion Members

		#region Constructor
		public CustomersController(ICustomerService customerService)
		{
			_customerService = customerService;
		}
		#endregion Constructor

		#region Methods
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCustomerById(Guid id)
		{
			var customer = await _customerService.GetCustomerByIdAsync(id);
			
			if (customer == null)
				return NotFound();

			return Ok(customer);
		}

		[HttpGet]
		public async Task<IActionResult> GetCustomers()
		{
			var customers = await _customerService.GetCustomersAsync();
			return Ok(customers);
		}

		[HttpPost]
		public async Task<IActionResult> AddCustomer([FromBody]Customer customer)
		{
			if (!IsValidCustomer(customer))
				return BadRequest("Invalid customer data.");

			await _customerService.AddCustomerAsync(customer);

			return Ok(new { Message = "Customer created successfully.", Customer = customer });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomer(Guid id,Customer customer)
		{
			if (!IsValidCustomer(customer))
				return BadRequest("Invalid customer data.");

			var result = await _customerService.UpdateCustomerAsync(id, customer);

			if (result == null)
				return NotFound();

			return Ok(new { Message = "Customer updated successfully.", Customer = customer });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(Guid id)
		{
			var result = await _customerService.DeleteCustomerAsync(id);

			if (result == null)
				return NotFound();

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
