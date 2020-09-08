namespace PaymentGateway.Services
{
	public class TestBankPaymentRequest
	{
		public decimal AmountInCents { get; set; }
		public string CardNumber { get; set; }
		public string Expiry { get; set; }
		public string CVV { get; set; }
		public string Reference { get; set; }

		public TestBankPaymentRequest(decimal amountInCents, string cardNumber, string expiry, string cVV, string reference)
		{
			AmountInCents = amountInCents;
			CardNumber = cardNumber;
			Expiry = expiry;
			CVV = cVV;
			Reference = reference;
		}
	}
}