using System;
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

		private void StopContainer(string containerName)
		{
			var process = new Process();
			process.StartInfo.FileName = "docker";
			process.StartInfo.Arguments = $"stop {containerName}";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.OutputDataReceived += (sender, data) =>
			{
				Console.WriteLine(data.Data);
			};
			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += (sender, data) =>
			{
				Console.WriteLine(data.Data);
			};
			process.Start();
		}

		private void StartContainer(string containerName)
		{
			var process = new Process();
			process.StartInfo.FileName = "docker";
			process.StartInfo.Arguments = $"start {containerName}";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.OutputDataReceived += (sender, data) =>
			{
				Console.WriteLine(data.Data);
			};
			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += (sender, data) =>
			{
				Console.WriteLine(data.Data);
			};
			process.Start();
		}
		protected void StartRedis()
		{
			StartContainer(RedisContainer);
		}
		protected void StopRedis()
		{
			StopContainer(RedisContainer);
		}


		protected void StartDatabase()
		{
			StartContainer(Database);
		}
		protected void StopDatabase()
		{
			StopContainer(Database);
		}

		protected void StartBank()
		{
			StartContainer(BankSimulator);
		}
		protected void StopBank()
		{
			StopContainer(BankSimulator);
		}

		protected void StartAll()
		{
			StartRedis();
			StartBank();
			StartDatabase();
		}
	}
}