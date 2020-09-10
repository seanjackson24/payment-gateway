using PaymentGateway.Common.Models;

namespace PaymentGateway.Domain.DomainModels
{
	public class BankResponse
	{
		public string BankReference { get; set; }
		public PaymentStatus Status { get; set; }
	}
}