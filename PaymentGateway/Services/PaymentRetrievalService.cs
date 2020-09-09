using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public interface IPaymentRetrievalService
	{
		Task<PaymentRetrievalResponse> GetPaymentInformation(string paymentId);
	}
	public class PaymentRetrievalService : IPaymentRetrievalService
	{
		private readonly IPaymentRepository _paymentRepository;

		public PaymentRetrievalService(IPaymentRepository paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}

		public async Task<PaymentRetrievalResponse> GetPaymentInformation(string paymentId)
		{
			var payment = await _paymentRepository.GetPaymentById(paymentId);

			if (payment != null)
			{
				return new PaymentRetrievalResponse()
				{
					PaymentStatus = payment.PaymentStatus.ToString(), // TODO
					MaskedCardNumber = payment.MaskedCardNumber,
					ExpiryDate = payment.CardExpiryDate,
				};
			}
			return null; // TODO: return type for when payment retrieval not found
						 //  return new PaymentRetrievalResponse() { PaymentStatus = PaymentStatus.NotFound };
		}
	}
}