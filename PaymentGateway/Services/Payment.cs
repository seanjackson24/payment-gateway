using System;

namespace PaymentGateway.Services
{
	public class Payment
	{
		public string PaymentId { get; set; }
		public string MaskedCardNumber { get; set; }
		public decimal PaymentAmount { get; set; }
		public DateTime TimestampUtc { get; set; }
		public int PaymentStatus { get; set; }
		public string BankReference { get; set; }

		public DateTime CardExpiryDate { get; set; }
		public object CardName { get; internal set; }
	}
}