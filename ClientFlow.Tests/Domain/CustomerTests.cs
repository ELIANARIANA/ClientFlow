using System;
using System.Collections.Generic;
using System.Text;

namespace ClientFlow.Tests.Domain
{
	public class CustomerTests
	{
		[Fact]
		public void CreateCustomerWithValidData()
		{
			var customer = new ClientFlow.Domain.Entities.Customer
			{
				Id          = Guid.NewGuid(),
				FirtName    = "John",
				LastName    = "Doe",
				Email       = "john.doe@example.com",
				Phone       = "123-456-7890",
				CompanyName = "Example Inc.",
				CreatedAt   = DateTimeOffset.UtcNow,
				UpdatedAt   = DateTimeOffset.UtcNow,
			};

			// Assert
			Assert.NotEqual(Guid.Empty            , customer.Id         );
			Assert.Equal   ("John"                , customer.FirtName   );
			Assert.Equal   ("Doe"                 , customer.LastName   );
			Assert.Equal   ("john.doe@example.com", customer.Email      );
			Assert.Equal   ("123-456-7890"        , customer.Phone      );
			Assert.Equal   ("Example Inc."        , customer.CompanyName);
			Assert.NotEqual(default               , customer.CreatedAt  );
			Assert.NotEqual(default               , customer.UpdatedAt  );
		}

		[Fact]
		public void CreateCustomerWithOnlyRequiredData()
		{
			var customer = new ClientFlow.Domain.Entities.Customer
			{ 
				Id        = Guid.NewGuid(),
				FirtName  = "John",
				LastName  = "Doe",
				Email     = "john.doe@example.com",
				CreatedAt = DateTimeOffset.UtcNow,
			};

			// Assert
			Assert.NotEqual(Guid.Empty            , customer.Id       );
			Assert.Equal   ("John"                , customer.FirtName );
			Assert.Equal   ("Doe"                 , customer.LastName );
			Assert.Equal   ("john.doe@example.com", customer.Email    );
			Assert.NotEqual(default               , customer.CreatedAt);
			Assert.Null(customer.Phone      );
			Assert.Null(customer.CompanyName);
			Assert.Null(customer.UpdatedAt  );
		}
	}
}
