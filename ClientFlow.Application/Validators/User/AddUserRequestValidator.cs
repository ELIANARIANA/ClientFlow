using FluentValidation;
using FluentValidation.Validators;

using ClientFlow.Application.DTOs.User;

namespace ClientFlow.Application.Validators.User
{
	public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
	{
		public AddUserRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("E-Mail must not be empty.")
				.EmailAddress(EmailValidationMode.AspNetCoreCompatible)
				.WithMessage("Please enter a valid E-Mail.")
				.MaximumLength(255)
				.WithMessage("E-Mail must be a maximum of 255 characters long.");
			RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage("Password must not be empty.")
				.MinimumLength(8)
				.WithMessage("Password must be a minimum of 8 characters long.")
				.MaximumLength(100)
				.WithMessage("Password must be a maximum of 100 characters long.");
		}
	}
}
