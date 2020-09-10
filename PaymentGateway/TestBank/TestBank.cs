using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
	public interface IBank
	{
		Task<BankResponse> PerformPayment(PaymentRequest request);
	}
	public class TestBank : IBank
	{
		private readonly TestBankHttpClient _bankHttpClient;

		public TestBank(TestBankHttpClient bankHttpClient)
		{
			_bankHttpClient = bankHttpClient;
		}

		public async Task<BankResponse> PerformPayment(PaymentRequest request)
		{
			var bankRequest = new TestBankPaymentRequest(request.PaymentAmountInCents, request.CardNumber, request.ExpiryDate, request.CVV, request.PaymentId);
			var result = await _bankHttpClient.RequestBankPayment(bankRequest, CancellationToken.None);

			return new BankResponse()
			{
				BankReference = result.BankReference,
				Status = result.WasSuccessfulPayment ? PaymentStatus.Accepted : PaymentStatus.Declined
			};
		}
	}
}