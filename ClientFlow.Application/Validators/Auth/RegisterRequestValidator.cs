using FluentValidation;
using FluentValidation.Validators;

using ClientFlow.Application.DTOs.Auth;

namespace ClientFlow.Application.Validators.Auth
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("E-Mail must not be empty.")
				.EmailAddress(EmailValidationMode.AspNetCoreCompatible)
				.EmailAddress(EmailValidationMode.Net4xRegex)
				.WithMessage("Pleas enter a valid E-Mail.")
				.MaximumLength(255)
				.WithMessage("E-Mail must be a maximum of 255 character long.");

			RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage("Password must not be empty")
				.MinimumLength(8)
				.WithMessage("Password must be a manimum of 8 character long")
				.MaximumLength(100)
				.WithMessage("Password must be a maximum of 100 character long");
		}
	}
}
