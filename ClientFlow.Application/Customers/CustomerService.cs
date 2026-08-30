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
	}
}
