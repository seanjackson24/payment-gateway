using PaymentGateway.Models;

namespace PaymentGateway.Services
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
				Status = PaymentStatusModel.Processing
			};
		}

		public PaymentResponse CreateResponse(string paymentId, PaymentActionResult actionResult)
		{
			return new PaymentResponse()
			{
				PaymentId = paymentId,
				Status = actionResult.WasPaymentAccepted ? PaymentStatusModel.Accepted : PaymentStatusModel.Declined
			};
		}
	}
}