using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
	public class PaymentRetrievalRequest
	{
		[Required]
		public string PaymentId { get; set; }
	}
}