using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Domain.TestBank
{
	public class TestBankHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public TestBankHttpClient(HttpClient client, IConfiguration configuration)
		{
			_httpClient = client;
			_configuration = configuration;
		}

		public async Task<TestBankPaymentResponse> RequestBankPayment(TestBankPaymentRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}
			string url = _configuration.GetValue<string>("BankSimulator.Url");

			var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(url, content, cancellationToken);

			var result = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			return JsonSerializer.Deserialize<TestBankPaymentResponse>(result, options);
		}
	}
}