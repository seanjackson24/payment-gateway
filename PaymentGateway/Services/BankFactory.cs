using System;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Services
{
	public interface IBankFactory
	{
		IBank GetBankByName(string bankName);
	}
	public class BankFactory : IBankFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public BankFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IBank GetBankByName(string bankName)
		{
			switch (bankName)
			{
				case Banks.TestBank:
					return _serviceProvider.GetService<TestBank>();
				default:
					throw new BankNotFoundException($"No Bank known by name ${bankName}");
			}
		}
	}
}