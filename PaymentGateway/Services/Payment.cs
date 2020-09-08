using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Services
{
	[Table("Payment")]
	public class Payment
	{
		public string PaymentId { get; set; }
		public string MaskedCardNumber { get; set; }
		public decimal PaymentAmount { get; set; }
		public DateTime TimestampUtc { get; set; }
		public int PaymentStatus { get; set; }
		public string BankReference { get; set; }

		public DateTime CardExpiryDate { get; set; }
		public string CardName { get; internal set; }
	}
}