namespace PaymentGateway.Common.Models
{
	public class PaymentRetrievalResponse
	{
		public string MaskedCardNumber { get; set; }
		// public PaymentStatus PaymentStatus { get; set; }
		public string PaymentStatus { get; set; }
		public string ExpiryDate { get; set; }
	}
}