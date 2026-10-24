using FluentValidation;
using FluentValidation.Validators;

using ClientFlow.Application.DTOs.Customer;

namespace ClientFlow.Application.Validators.Customer
{
	public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
	{
		public CustomerRequestValidator()
		{
			RuleFor(x => x.FirstName)
				.NotEmpty()
				.WithMessage("First name must not be empty.")
				.MaximumLength(100)
				.WithMessage("First name must be a maximum of 100 characters long.");

			RuleFor(x => x.LastName)
				.NotEmpty()
				.WithMessage("Last name must not be empty.")
				.MaximumLength(100)
				.WithMessage("Last name must be a maximum of 100 characters long.");

			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("E-Mail must not be empty.")
				.EmailAddress(EmailValidationMode.AspNetCoreCompatible)
				.EmailAddress(EmailValidationMode.Net4xRegex)
				.WithMessage("Pleas enter a valid E-Mail.")
				.MaximumLength(255)
				.WithMessage("E-Mail must be a maximum of 255 character long.");

			RuleFor(x => x.Phone)
				.MaximumLength(20)
				.WithMessage("Phone number must be a maximum of 20 characters long.");

			RuleFor(x => x.CompanyName)
				.MaximumLength(255)
				.WithMessage("Company name must be a maximum of 255 characters long.");
		}
	}
}
