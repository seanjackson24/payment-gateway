namespace PaymentGateway.Common.Models
{
	public class PaymentResponse
	{
		/// <summary>
		/// A unique ID for this payment, to prevent duplicates.
		/// </summary>
		public string PaymentId { get; set; }

		public PaymentStatus Status { get; set; }
	}
}
