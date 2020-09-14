using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.DomainModels;

namespace PaymentGateway.Domain.Services
{
	public class PaymentGatewayDbContext : DbContext
	{
		public PaymentGatewayDbContext([NotNull] DbContextOptions options) : base(options)
		{
		}

		public DbSet<Payment> Payments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Payment>()
				.Property(p => p.PaymentId).HasMaxLength(100);
			modelBuilder.Entity<Payment>()
				.Property(p => p.MaskedCardNumber).HasMaxLength(20);
			modelBuilder.Entity<Payment>()
				.Property(p => p.BankReference).HasMaxLength(100);
			modelBuilder.Entity<Payment>()
				.Property(p => p.CardExpiryDate).HasMaxLength(5);
			modelBuilder.Entity<Payment>()
				.Property(p => p.CurrencyCode).HasMaxLength(3);
			modelBuilder.Entity<Payment>()
				.Property(p => p.PaymentStatus).HasConversion<int>();

			base.OnModelCreating(modelBuilder);
		}
	}
}