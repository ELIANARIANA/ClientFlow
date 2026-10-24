using System.ComponentModel.DataAnnotations;

namespace ClientFlow.Application.DTOs.Customer
{
	public class AddCustomerRequest
	{
		public string FirstName    { get; set; } = string.Empty;
		public string LastName     { get; set; } = string.Empty;
		[EmailAddress]
		public string Email        { get; set; } = string.Empty;
		public string? Phone       { get; set; }
		public string? CompanyName { get; set; }
	}
}
