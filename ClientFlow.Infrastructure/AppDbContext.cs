using Microsoft.EntityFrameworkCore;

using ClientFlow.Domain.Entities;

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
		/// Gets or sets the Users DbSet.
		/// </summary>
		public DbSet<User> Users { get; set; }

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
				entity.Property(x => x.Id).UseIdentityByDefaultColumn();
				entity.Property(x => x.CustomerId).IsRequired();
				entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
				entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
				entity.Property(x => x.Email).HasMaxLength(254).IsRequired();
				entity.Property(x => x.Phone).HasMaxLength(20);
				entity.Property(x => x.CompanyName).HasMaxLength(200);
				entity.Property(x => x.CreatedAt).IsRequired();
				entity.Property(x => x.UpdatedAt);
			});
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.Id).UseIdentityByDefaultColumn();
				entity.Property(x => x.UserId).IsRequired();
				entity.Property(x => x.Email).HasMaxLength(254).IsRequired();
				entity.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();
				entity.Property(x => x.Role).IsRequired();
				entity.Property(x => x.CreatedAt).IsRequired();
				entity.Property(x => x.UpdatedAt);
				entity.HasIndex(x => x.Email).IsUnique();
			});
		}
	}
}