using Microsoft.EntityFrameworkCore;

using ClientFlow.Application.Customers;
using ClientFlow.Infrastructure;
using ClientFlow.Infrastructure.Repositories;

namespace ClientFlow.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{ 
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseNpgsql(
					builder.Configuration.GetConnectionString("DefaultConnection")));

			// Add services to the container.
			builder.Services.AddControllers();

			builder.Services.AddOpenApi();

			builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

			var app = builder.Build();

			// Apply pending migrations at startup
			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider
					.GetRequiredService<AppDbContext>();

				dbContext.Database.Migrate();
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
