using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public interface IPaymentResponseFactory
	{
		PaymentResponse CreateProcessingResponse(string paymentId);
		PaymentResponse CreateResponse(string paymentId, int actionResult);
	}
	public class PaymentResponseFactory : IPaymentResponseFactory
	{
		public PaymentResponse CreateProcessingResponse(string paymentId)
		{
			return new PaymentResponse()
			{
				PaymentId = paymentId,
				Status = PaymentStatus.Processing.ToString()

			};
		}

		public PaymentResponse CreateResponse(string paymentId, int actionResult)
		{
			return new PaymentResponse()
			{
				// TODO:
				PaymentId = paymentId,
				Status = actionResult == 4 ? PaymentStatus.Accepted.ToString() : PaymentStatus.Declined.ToString()
			};
		}
	}
}