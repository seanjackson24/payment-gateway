using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public interface IPaymentRetrievalService
	{
		Task<PaymentRetrievalResponse> GetPaymentInformation(string paymentId);
	}
	public class PaymentRetrievalService : IPaymentRetrievalService
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IPaymentRepository _paymentRepository;

		public PaymentRetrievalService(IMemoryCache memoryCache, IPaymentRepository paymentRepository)
		{
			_memoryCache = memoryCache;
			_paymentRepository = paymentRepository;
		}

		public async Task<PaymentRetrievalResponse> GetPaymentInformation(string paymentId)
		{
			if (_memoryCache.TryGetValue<PaymentRetrievalResponse>(paymentId, out var paymentResult) && paymentResult != null)
			{
				return paymentResult;
			}

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
			return null; // TODO: return type
						 //  return new PaymentRetrievalResponse() { PaymentStatus = PaymentStatus.NotFound };
		}
	}
}