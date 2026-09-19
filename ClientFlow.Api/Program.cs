using System.Text;

using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.IdentityModel.Tokens;

using ClientFlow.Application.Customers;
using ClientFlow.Application.Interfaces;
using ClientFlow.Application.Services;
using ClientFlow.Infrastructure;
using ClientFlow.Infrastructure.Repositories;
using ClientFlow.Infrastructure.Security;
using ClientFlow.Api.Middleware;

namespace ClientFlow.Api
{
	/// <summary>
	/// The main entry point for the ClientFlow API application.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <param name="args"></param>
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

			// Add global exception handler
			builder.Services.AddProblemDetails();
			builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

			// Add application services and repositories
			builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
			builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
			builder.Services.AddScoped<AuthService>();

			// Authentication
			builder.Services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer           = true,
						ValidateAudience         = true,
						ValidateLifetime         = true,
						ValidateIssuerSigningKey = true,

						ValidIssuer   = builder.Configuration["Jwt:Issuer"],
						ValidAudience = builder.Configuration["Jwt:Audience"],

						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(
								builder.Configuration["Jwt:Key"]!
							)
						)
					};
				});

			// Authorization
			builder.Services.AddAuthorization();

			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen(options =>
			{
				var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

				if (File.Exists(xmlPath))
					options.IncludeXmlComments(xmlPath);

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					
					Name         = "Authorization",
					Type         = SecuritySchemeType.Http,
					Scheme       = "Bearer",
					BearerFormat = "JWT",
					In           = ParameterLocation.Header,
					Description  = "Enter 'Bearer {Token}' (e.g. 'Bearer eyJhbG...')",
				});

				options.AddSecurityRequirement(document =>
					new OpenApiSecurityRequirement
					{
						[new OpenApiSecuritySchemeReference("Bearer", document)] = []
					});
			});

			var app = builder.Build();

			app.UseSerilogRequestLogging();

			app.Logger.LogInformation("=======================================");
			app.Logger.LogInformation("ClientFlow API starting...");
			app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

			var configuredUrls = app.Configuration["urls"]
					 ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
					 ?? "http://localhost:5000 (Default)";

			app.Logger.LogInformation("Application will be launched at: {Urls}", configuredUrls);

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

			app.UseExceptionHandler();

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Logger.LogInformation("ClientFlow API started successfully.");
			app.Logger.LogInformation("=======================================");

			app.Run();
		}
	}
}
