using PaymentGateway.Common.Models;
using PaymentGateway.Domain.DomainModels;

namespace PaymentGateway.Domain.Factories
{
	public interface IPaymentResponseFactory
	{
		PaymentResponse CreateProcessingResponse(string paymentId);
		PaymentResponse CreateResponse(string paymentId, PaymentActionResult actionResult);
	}
	public class PaymentResponseFactory : IPaymentResponseFactory
	{
		public PaymentResponse CreateProcessingResponse(string paymentId)
		{
			return new PaymentResponse()
			{
				PaymentId = paymentId,
				Status = PaymentStatus.Processing
			};
		}

		public PaymentResponse CreateResponse(string paymentId, PaymentActionResult actionResult)
		{
			return new PaymentResponse()
			{
				PaymentId = paymentId,
				Status = actionResult.WasPaymentAccepted ? PaymentStatus.Accepted : PaymentStatus.Declined
			};
		}
	}
}