using System;

namespace PaymentGateway.Models
{
	public class PaymentRetrievalResponse
	{
		public string MaskedCardNumber { get; set; }

		public string PaymentStatus { get; set; }
		public DateTime ExpiryDate { get; internal set; }
		public object CardName { get; internal set; }
	}
}