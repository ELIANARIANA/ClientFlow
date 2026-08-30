using ClientFlow.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace ClientFlow.Infrastructure
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Customer> Customers { get; set; }

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