using FluentValidation.TestHelper;

using ClientFlow.Application.DTOs.Auth;
using ClientFlow.Application.Validators.Auth;

namespace ClientFlow.Tests.Validators.Auth
{
	public class RegisterRequestValidatorTests
	{
		#region Memebers
		private readonly RegisterRequestValidator _registerReqValidator;
		#endregion Members

		#region Constructor
		public RegisterRequestValidatorTests()
		{
			_registerReqValidator = new RegisterRequestValidator();
		}
		#endregion Constructor

		[Fact]
		public void ShouldNotHaveErrorWhenRequestIsValid()
		{
			var request = new RegisterRequest
			{
				Email    = "user@example.com",
				Password = "Password123!",
			};

			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Fact]
		public void ShouldHaveErrorWhenEmailIsEmpty()
		{
			var request = new RegisterRequest
			{
				Email    = "",
				Password = "Password123!",
			};

			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email);
		}

		[Fact]
		public void ShouldHaveErrorWhenEmailIsInvalid()
		{
			var request = new RegisterRequest
			{
				Email    = "user@",
				Password = "Password123!",
			};

			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email);
		}

		[Fact]
		public void ShouldHaveErrorWhenEmailIsTooLong()
		{
			var request = new RegisterRequest
			{
				Email    = new string('a', 250 ) + "user@",
				Password = "Password123!",
			};

			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email);
		}

		[Fact]
		public void ShowHaveErrorWhenPasswordIsEmpty()
		{
			var request = new RegisterRequest()
			{
				Email    = "user@example.com",
				Password = "",
			};

			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password);
		}

		[Fact]
		public void ShowHaveErrorWhenPasswordIsTooShort()
		{
			var request = new RegisterRequest()
			{
				Email    = "user@example.com",
				Password = "string",
			};

			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password);
		}

		[Fact]
		public void ShowHaveErrorWhenPasswordIsTooLong()
		{
			var request = new RegisterRequest()
			{
				Email    = "user@example.com",
				Password = new String('a', 101),
			};
			
			// Act
			var result = _registerReqValidator.TestValidate(request);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password);
		}
	}
}
