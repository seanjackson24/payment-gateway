namespace PaymentGateway.Domain.DomainModels
{
	public class PaymentActionResult
	{
		public bool WasPaymentAccepted { get; }

		public PaymentActionResult(bool wasPaymentAccepted)
		{
			WasPaymentAccepted = wasPaymentAccepted;
		}
	}
}