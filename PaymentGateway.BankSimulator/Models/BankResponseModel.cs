using System;
namespace PaymentGateway.BankSimulator.Models
{
	public class BankResponseModel
	{
		public string BankReference { get; private set; }
		public bool WasSuccessfulPayment { get; private set; }

		// TODO: Bank Response Model
		public static BankResponseModel Accepted = new BankResponseModel() { BankReference = Guid.NewGuid().ToString(), WasSuccessfulPayment = true };
		public static BankResponseModel Declined = new BankResponseModel() { BankReference = Guid.NewGuid().ToString(), WasSuccessfulPayment = false };
	}
}