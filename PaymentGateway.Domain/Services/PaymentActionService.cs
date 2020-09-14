using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.DomainModels;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Services
{
	public interface IPaymentActionService
	{
		Task<PaymentActionResult> PerformPayment(PaymentRequest request, CancellationToken cancellationToken);
	}
	public class PaymentActionService : IPaymentActionService
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly ICardMaskingService _cardMaskingService;
		private readonly ITimeProvider _timeProvider;
		private readonly IBankService _bankService;
		private readonly ILogger<PaymentActionService> _logger;

		public PaymentActionService(IPaymentRepository paymentRepository, ICardMaskingService cardMaskingService, ITimeProvider timeProvider, IBankService bankService, ILogger<PaymentActionService> logger)
		{
			_paymentRepository = paymentRepository;
			_cardMaskingService = cardMaskingService;
			_timeProvider = timeProvider;
			_bankService = bankService;
			_logger = logger;
		}

		public async Task<PaymentActionResult> PerformPayment(PaymentRequest request, CancellationToken cancellationToken)
		{
			if (await _paymentRepository.PaymentExists(request.PaymentId, cancellationToken))
			{
				throw new PaymentAlreadyExistsException();
			}

			BankResponse bankResponse = await _bankService.PerformPaymentAsync(request);

			try
			{
				var payment = new Payment()
				{
					MaskedCardNumber = _cardMaskingService.MaskCardNumber(request.CardNumber),
					PaymentAmountInCents = request.PaymentAmountInCents,
					PaymentId = request.PaymentId,
					TimestampUtc = _timeProvider.UtcNow(),
					PaymentStatus = bankResponse.Status,
					BankReference = bankResponse.BankReference,
					CardExpiryDate = request.ExpiryDate,
					CurrencyCode = request.CurrencyCode
				};
				await _paymentRepository.Insert(payment);
			}
			catch (Exception ex)
			{
				// if we perform the payment, but error afterwards, we still want to respond to the client, but log it
				_logger.LogCritical(ex, "Unable to write payment information to database after payment has been taken", request.PaymentId, bankResponse);
			}

			return new PaymentActionResult(bankResponse.Status == PaymentStatus.Accepted);
		}
	}
}