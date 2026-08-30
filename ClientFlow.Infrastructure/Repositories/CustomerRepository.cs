using ClientFlow.Application.Customers;

namespace ClientFlow.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		#region Members
		private readonly AppDbContext _context;
		#endregion Members

		#region Constructor
		public CustomerRepository(AppDbContext context)
		{
			_context = context;
		}
		#endregion Constructor
	}
}
