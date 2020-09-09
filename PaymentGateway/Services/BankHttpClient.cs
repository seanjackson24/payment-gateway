using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Services
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
			var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
			var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

			using (var contentStream = await response.Content.ReadAsStreamAsync())
			{
				// return await as opposed to just return as we are inside a using statement
				return await JsonSerializer.DeserializeAsync<TestBankPaymentResponse>(contentStream, new JsonSerializerOptions(), cancellationToken);
			}
		}
	}
}