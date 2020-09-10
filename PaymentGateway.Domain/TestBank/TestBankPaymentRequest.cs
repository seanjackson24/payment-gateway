namespace PaymentGateway.Domain.TestBank
{
	public class TestBankPaymentRequest
	{
		public decimal AmountInCents { get; }
		public string CardNumber { get; }
		public string Expiry { get; }
		public string CVV { get; }
		public string Reference { get; }

		public TestBankPaymentRequest(decimal amountInCents, string cardNumber, string expiry, string cvv, string reference)
		{
			AmountInCents = amountInCents;
			CardNumber = cardNumber;
			Expiry = expiry;
			CVV = cvv;
			Reference = reference;
		}
	}
}