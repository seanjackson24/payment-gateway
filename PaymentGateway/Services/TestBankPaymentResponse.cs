namespace PaymentGateway.Services
{
	public class TestBankPaymentResponse
	{
		public string BankReference { get; internal set; }
		public bool WasSuccessfulPayment { get; internal set; }
	}
}