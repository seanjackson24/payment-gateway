using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Services
{
	public class PaymentGatewayDbContext : DbContext
	{
		public PaymentGatewayDbContext([NotNullAttribute] DbContextOptions options) : base(options)
		{
		}

		public PaymentGatewayDbContext()
		{

		}

		public DbSet<Payment> Payments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Payment>()
				.Property(p => p.PaymentStatus).HasConversion<int>();
			base.OnModelCreating(modelBuilder);
		}
	}
}