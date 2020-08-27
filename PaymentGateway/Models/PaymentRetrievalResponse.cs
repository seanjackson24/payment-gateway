namespace PaymentGateway.Models
{
	public class PaymentRetrievalResponse
	{
		public string MaskedCardNumber { get; set; }

		public string PaymentStatus { get; set; }
	}
}