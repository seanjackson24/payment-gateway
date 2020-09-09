using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public interface IPaymentActionService
	{
		Task<int> PerformPayment(PaymentRequest request, CancellationToken cancellationToken);
	}
	public class PaymentActionService : IPaymentActionService
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly ICardMaskingService _cardMaskingService;
		private readonly ITimeProvider _timeProvider;
		private readonly IBankService _bankService;

		public PaymentActionService(IPaymentRepository paymentRepository, ICardMaskingService cardMaskingService, ITimeProvider timeProvider, IBankService bankService)
		{
			_paymentRepository = paymentRepository;
			_cardMaskingService = cardMaskingService;
			_timeProvider = timeProvider;
			_bankService = bankService;
		}

		public async Task<int> PerformPayment(PaymentRequest request, CancellationToken cancellationToken)
		{
			// TODO: cancellation token
			if (await _paymentRepository.PaymentExists(request.PaymentId, cancellationToken))
			{
				// return PaymentAlreadyExists; // TODO
				return -1;
			}
			// Store in DB? - no - could do this with SQL server encryption if you wanted to add background process / recovery
			// (cont.) which makes the entire thing more complicated as then you would need something like a failsafe URL etc

			// determine bank
			BankResponse bankResponse = await _bankService.PerformPaymentAsync(request);
			// execute to bank

			// update DB
			var payment = new Payment()
			{
				MaskedCardNumber = _cardMaskingService.MaskCardNumber(request.CardNumber),
				PaymentAmount = request.PaymentAmountInCents,
				PaymentId = request.PaymentId,
				TimestampUtc = _timeProvider.UtcNow(),
				PaymentStatus = (int)bankResponse.Status,
				BankReference = bankResponse.BankReference,
				CardExpiryDate = request.ExpiryDate,
			};
			await _paymentRepository.Insert(payment, cancellationToken);
			return await Task.FromResult(4);
		}
	}
}