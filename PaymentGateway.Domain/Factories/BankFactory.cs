using System;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.TestBank;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Domain.Factories
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
			return bankName switch
			{
				Banks.TestBank => _serviceProvider.GetService<TestBank.TestBank>(),
				_ => throw new BankNotFoundException($"No Bank known by name ${bankName}")
			};
		}
	}
}