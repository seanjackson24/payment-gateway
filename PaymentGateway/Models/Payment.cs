namespace PaymentGateway.Models
{
	public class Payment
	{
		public string PaymentId { get; private set; }
		public string CardNumber { get; private set; }
		public string ExpiryDate { get; private set; }
		public string CVV { get; private set; }
		public decimal PaymentAmount { get; private set; }
		public string CurrencyCode { get; private set; }

		public PaymentStatus Status { get; set; }
	}
}