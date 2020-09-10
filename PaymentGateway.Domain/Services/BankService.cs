using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.DomainModels;
using PaymentGateway.Domain.Factories;

namespace PaymentGateway.Domain.Services
{
	public interface IBankService
	{
		Task<BankResponse> PerformPaymentAsync(PaymentRequest request);
	}
	public class BankService : IBankService
	{
		private readonly IConfiguration _configuration;
		private readonly IBankFactory _bankFactory;

		public BankService(IConfiguration configuration, IBankFactory bankFactory)
		{
			_configuration = configuration;
			_bankFactory = bankFactory;
		}

		public async Task<BankResponse> PerformPaymentAsync(PaymentRequest request)
		{
			var bank = _bankFactory.GetBankByName(_configuration.GetValue<string>("BankName"));
			return await bank.PerformPayment(request);
		}
	}
}