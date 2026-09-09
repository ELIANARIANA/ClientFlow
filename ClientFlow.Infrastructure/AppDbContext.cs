using ClientFlow.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace ClientFlow.Infrastructure
{
	/// <summary>
	/// Represents the application's database context for Entity Framework Core.
	/// </summary>
	public class AppDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppDbContext"/> class.
		/// </summary>
		/// <param name="options"></param>
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		/// <summary>
		/// Gets or sets the Customers DbSet.
		/// </summary>
		public DbSet<Customer> Customers { get; set; }


		/// <summary>
		/// Configures the model for the context.
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Customer>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.FirstName).IsRequired();
				entity.Property(x => x.LastName).IsRequired();
				entity.Property(x => x.Email).IsRequired();
				entity.Property(x => x.Phone);
				entity.Property(x => x.CompanyName);
				entity.Property(x => x.CreatedAt).IsRequired();
				entity.Property(x => x.UpdatedAt);
			});
		}
	}
}