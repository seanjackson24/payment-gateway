using System;
using System.Threading.Tasks;
using PaymentGateway.Common.Models;

namespace PaymentGateway.Domain.Services
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
					PaymentStatus = (PaymentStatus)payment.PaymentStatus,
					MaskedCardNumber = payment.MaskedCardNumber,
					ExpiryDate = payment.CardExpiryDate,
				};
			}
			return null;
		}
	}
}