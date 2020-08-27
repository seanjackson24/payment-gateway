using System;
using PaymentGateway.Models;

namespace PaymentGateway.Factories
{
	public class BankFactory
	{
		public Bank GetBankFromCardNumber(string cardNumber)
		{
			switch (cardNumber)
			{
				case "4111111111111111":
					return new Bank()
					{
						Name = "Imaginary Bank"
					};
				default:
					throw new NotImplementedException();
			}
		}
	}
}