using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PaymentGateway.Tests.IntegrationTests
{
	public class EndToEndTests : EndToEndTestBase
	{
		public EndToEndTests(ITestOutputHelper output) : base(output)
		{
		}

		//[Fact]
		//public async Task StartAsync()
		//{
		//	Output.WriteLine("hello");
		//	await StartRedis();
		//	await StopRedis();
		//}



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