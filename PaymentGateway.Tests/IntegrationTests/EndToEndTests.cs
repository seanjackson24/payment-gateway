using System.Threading.Tasks;

namespace PaymentGateway.Tests.IntegrationTests
{
	public class EndToEndTests
	{
		private async Task StartRedis()
		{
			// docker start 650b780b58a6

			await Task.Delay(1000);
		}
		private async Task StopRedis()
		{
			// docker stop 650b780b58a6

			await Task.Delay(1000);
		}


		private async Task StartDatabase()
		{
			// docker start 4b4e77063ce6

			await Task.Delay(1000);
		}
		private async Task StopDatabase()
		{
			// docker stop 4b4e77063ce6

			await Task.Delay(1000);
		}



		private async Task StartBank()
		{
			// dotnet run

			await Task.Delay(1000);
		}
		private async Task StopBank()
		{
			// TODO: put it in container

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