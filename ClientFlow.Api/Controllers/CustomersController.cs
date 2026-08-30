using Microsoft.AspNetCore.Mvc;

using ClientFlow.Application.Customers;

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
	}
}
