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
			// Validate the customer object
			if (customer == null                         ||
				string.IsNullOrEmpty(customer.FirstName) ||
				string.IsNullOrEmpty(customer.LastName ) ||
				string.IsNullOrEmpty(customer.Email    ))
			{
				return BadRequest("Invalid customer data.");
			}

			await _customerService.AddCustomerAsync(customer);

			return Ok(new { Message = "Customer created successfully.", Customer = customer });
		}
		#endregion Methods
	}
}
