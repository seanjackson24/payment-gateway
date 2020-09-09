namespace PaymentGateway.Models
{
	public class BankResponse
	{
		public string BankReference { get; set; }
		public PaymentStatus Status { get; set; }
	}
}