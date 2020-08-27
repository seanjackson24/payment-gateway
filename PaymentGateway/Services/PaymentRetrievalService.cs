using System;
using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public class PaymentRetrievalService
	{
		private readonly IMemoryCache _memoryCache;

		public PaymentRetrievalService(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public PaymentRetrievalResponse GetPaymentInformation(string paymentId)
		{
			if (_memoryCache.TryGetValue<PaymentRetrievalResponse>(paymentId, out var paymentResult) && paymentResult != null)
			{
				return paymentResult;
			}

			throw new NotImplementedException();
		}
	}
}