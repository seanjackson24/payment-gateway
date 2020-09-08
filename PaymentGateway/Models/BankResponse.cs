namespace PaymentGateway.Models
{
	public class BankResponse
	{
		public string BankReference { get; set; }
		public PaymentStatus Status { get; set; }
		public string Reference { get; set; }
	}
}