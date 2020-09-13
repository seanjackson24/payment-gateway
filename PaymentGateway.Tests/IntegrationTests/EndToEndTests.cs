using System.Threading.Tasks;

namespace PaymentGateway.Tests.IntegrationTests
{
	public class EndToEndTests
	{
		private async Task StartRedis()
		{
			// docker start PaymentGateway.Redis

			await Task.Delay(1000);
		}
		private async Task StopRedis()
		{
			// docker stop PaymentGateway.Redis

			await Task.Delay(1000);
		}


		private async Task StartDatabase()
		{
			// docker start PaymentGateway.Database

			await Task.Delay(1000);
		}
		private async Task StopDatabase()
		{
			// docker stop PaymentGateway.Database

			await Task.Delay(1000);
		}



		private async Task StartBank()
		{
			// docker start PaymentGateway.BankSimulator

			await Task.Delay(1000);
		}
		private async Task StopBank()
		{
			// docker stop PaymentGateway.BankSimulator

			await Task.Delay(1000);
		}





		// happy path: accepted
		// happy path: declined
		// happy path: request payment
		// happy path: request declined payment
		// unhappy path: request not found payment
		// unhappy path: request two payments at once
		// unhappy path: request same payment after completed
		// unhappy path: request negative payment
		// unhappy path: request not credit card number
		// unhappy path: CVV validation
		// unhappy path: expiry validation
		// unhappy path: unknown currency code
		// unhappy path: no payment ID
	}
}