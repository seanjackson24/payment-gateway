using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Models;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Services
{
	public class PaymentService
	{
		private readonly ILockActionService _lockService;
		private readonly IPaymentRepository _paymentRepository;
		private readonly ICardMaskingService _cardMaskingService;
		private readonly ITimeProvider _timeProvider;
		private readonly IConfiguration _configuration;
		private readonly IPaymentResponseFactory _paymentResponseFactory;
		private readonly IBankService _bankService;

		public PaymentService(ILockActionService lockActionService, ICardMaskingService cardMaskingService, IPaymentRepository paymentRepository, IConfiguration configuration, IPaymentResponseFactory paymentResponseFactory, IBankService bankService, ITimeProvider timeProvider)
		{
			_lockService = lockActionService;
			_cardMaskingService = cardMaskingService;
			_paymentRepository = paymentRepository;
			_configuration = configuration;
			_paymentResponseFactory = paymentResponseFactory;
			_bankService = bankService;
			_timeProvider = timeProvider;
		}
		public async Task<PaymentResponse> PerformPayment(PaymentRequest request, CancellationToken cancellationToken)
		{
			// TODO:
			Func<Task<int>> action = async () =>
			{
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
					PaymentStatus = (int)bankResponse.Status, // TODO
					BankReference = bankResponse.Reference
				};
				await _paymentRepository.Insert(payment, cancellationToken);
				return await Task.FromResult(4);
			};

			var timeout = TimeSpan.FromMilliseconds(_configuration.GetValue("PaymentLockTimeoutMilliseconds", 1000));
			var lockAction = new LockAction<int>(request.PaymentId, timeout, action);
			var result = await _lockService.TryExecuteLockAction(lockAction);
			if (!result.WasSuccessful)
			{
				return _paymentResponseFactory.CreateProcessingResponse(request.PaymentId);
			}
			return _paymentResponseFactory.CreateResponse(request.PaymentId, result.ActionResult);
		}
	}
}