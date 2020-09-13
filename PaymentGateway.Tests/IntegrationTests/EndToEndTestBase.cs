using System.Diagnostics;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace PaymentGateway.Tests.IntegrationTests
{
	public abstract class EndToEndTestBase
	{
		private const string RedisContainer = "PaymentGateway.Redis";
		private const string Database = "PaymentGateway.Database";
		private const string BankSimulator = "PaymentGateway.BankSimulator";

		protected readonly ITestOutputHelper Output;

		protected EndToEndTestBase(ITestOutputHelper output)
		{
			Output = output;
		}

		private async Task StopContainer(string containerName)
		{
			var process = new Process();
			process.StartInfo.FileName = "docker";
			process.StartInfo.Arguments = $"stop ${containerName}";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.Start();
			await Task.Delay(1000);
		}

		private async Task StartContainer(string containerName)
		{
			var process = new Process();
			process.StartInfo.FileName = "docker";
			process.StartInfo.Arguments = $"start {containerName}";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.OutputDataReceived += (sender, data) =>
			{
				Output.WriteLine(data.Data);
			};
			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += (sender, data) =>
			{
				Output.WriteLine(data.Data);
			};
			process.Start();
			await Task.Delay(1000);
		}
		protected async Task StartRedis()
		{
			await StartContainer(RedisContainer);
		}
		protected async Task StopRedis()
		{
			await StopContainer(RedisContainer);
		}


		protected async Task StartDatabase()
		{
			await StartContainer(Database);
		}
		protected async Task StopDatabase()
		{
			await StopContainer(Database);
		}

		protected async Task StartBank()
		{
			await StartContainer(BankSimulator);
		}
		protected async Task StopBank()
		{
			await StopContainer(BankSimulator);
		}
	}
}