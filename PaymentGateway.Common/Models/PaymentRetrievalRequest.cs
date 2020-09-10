using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Common.Models
{
	public class PaymentRetrievalRequest
	{
		[Required]
		public string PaymentId { get; set; }
	}
}