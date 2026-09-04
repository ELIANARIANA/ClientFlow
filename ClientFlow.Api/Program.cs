using Serilog;
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

			// Configure Serilog
			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(context.Configuration));

			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseNpgsql(
					builder.Configuration.GetConnectionString("DefaultConnection")));

			// Add services to the container.
			builder.Services.AddControllers();

			builder.Services.AddOpenApi();

			// Add application services and repositories
			builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			app.UseSerilogRequestLogging();

			app.Logger.LogInformation("=======================================");
			app.Logger.LogInformation("ClientFlow API starting...");
			app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

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

			// Add Swagger UI
			if(app.Configuration.GetValue<bool>("Swagger:Enabled"))
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientFlow API v1");
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Logger.LogInformation("ClientFlow API started successfully.");
			app.Logger.LogInformation("=======================================");

			app.Run();
		}
	}
}
