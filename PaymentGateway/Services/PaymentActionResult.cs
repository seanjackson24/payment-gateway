namespace PaymentGateway.Services
{
	public class PaymentActionResult
	{
		public bool WasPaymentAccepted { get; set; }

		public PaymentActionResult(bool wasPaymentAccepted)
		{
			WasPaymentAccepted = wasPaymentAccepted;
		}
	}
}