using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Models;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Services
{
	public interface IPaymentService
	{
		Task<PaymentResponse> PerformPayment(PaymentRequest request, CancellationToken cancellationToken);
	}
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly ILockActionService _lockService;
		private readonly IPaymentResponseFactory _paymentResponseFactory;
		private readonly IPaymentActionService _paymentActionService;

		public PaymentService(ILockActionService lockActionService, IConfiguration configuration, IPaymentResponseFactory paymentResponseFactory, IPaymentActionService paymentActionService)
		{
			_lockService = lockActionService;
			_paymentResponseFactory = paymentResponseFactory;
			_configuration = configuration;
			_paymentActionService = paymentActionService;
		}
		public async Task<PaymentResponse> PerformPayment(PaymentRequest request, CancellationToken cancellationToken)
		{
			// TODO action return type:
			Func<Task<int>> action = async () =>
			{
				return await _paymentActionService.PerformPayment(request, cancellationToken);
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