using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public class PaymentService
	{
		private readonly ILockActionService _lockService;

		public PaymentService(ILockActionService lockActionService)
		{
			_lockService = lockActionService;
		}
		public async Task<PaymentResponse> PerformPayment(PaymentRequest request)
		{
			// TODO:
			Func<Task<int>> action = async () => await Task.FromResult(4);

			var lockAction = new LockAction<int>(request.PaymentId, TimeSpan.FromSeconds(1), action);
			var result = await _lockService.TryExecuteLockAction(lockAction);
			if (!result.WasSuccessful)
			{
				// TODO:
				return new PaymentResponse() { Status = PaymentStatus.Processing.ToString() };

			}
			else
			{
				return new PaymentResponse()
				{
					// TODO:
					PaymentId = request.PaymentId,
					Status = result.ActionResult == 4 ? PaymentStatus.Accepted.ToString() : PaymentStatus.Declined.ToString()
				};
			}
		}
	}
}