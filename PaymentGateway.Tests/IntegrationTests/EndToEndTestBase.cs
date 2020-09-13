using System.Diagnostics;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace PaymentGateway.Tests.IntegrationTests
{
	public abstract class EndToEndTestBase
	{
		private const string redisContainer = "PaymentGateway.Redis";
		private const string database = "PaymentGateway.Database";
		private const string BankSimulator = "PaymentGateway.BankSimulator";

		protected readonly ITestOutputHelper _output;

		protected EndToEndTestBase(ITestOutputHelper output)
		{
			_output = output;
		}

		private async Task Stopcontainer(string containerName)
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
				_output.WriteLine(data.Data);
			};
			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += (sender, data) =>
			{
				_output.WriteLine(data.Data);
			};
			process.Start();
			await Task.Delay(1000);
		}
		protected async Task StartRedis()
		{
			await StartContainer(redisContainer);
		}
		protected async Task StopRedis()
		{
			await Stopcontainer(redisContainer);
		}


		protected async Task StartDatabase()
		{
			await StartContainer(database);
		}
		protected async Task StopDatabase()
		{
			await Stopcontainer(database);
		}

		protected async Task StartBank()
		{
			await StartContainer(BankSimulator);
		}
		protected async Task StopBank()
		{
			await Stopcontainer(BankSimulator);
		}
	}
}