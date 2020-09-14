using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Xunit;

namespace PaymentGateway.Tests.IntegrationTests
{
    public abstract class EndToEndTestBase
	{
		private const string RedisContainer = "PaymentGateway.Redis";
		private const string Database = "paymentgateway.database";
		private const string BankSimulator = "PaymentGateway.BankSimulator";

		protected const string acceptedCard = "4111111111111111";
		protected const string DeclinedCardNumber = "5105105105105100";
		protected const string CurrencyCode = "GBP";
		protected readonly IConfigurationRoot _configuration;

		public EndToEndTestBase()
        {
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			StartAll();
			// Make sure you check the bank simulator URL - particularly if you are running it in a container vs dotnet run
			var initialData = new List<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("PaymentGatewayRootUrl", "https://localhost:5001")
			};
			_configuration = new ConfigurationBuilder().AddInMemoryCollection(initialData).Build();
		}

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
			Thread.Sleep(5000);
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
			Thread.Sleep(1000);
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

	[CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
	public class NonParallelCollectionDefinitionClass
	{
	}
}