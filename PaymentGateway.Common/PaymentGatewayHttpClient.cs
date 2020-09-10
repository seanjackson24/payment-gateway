using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Common.Models;

namespace PaymentGateway.Common
{
	public interface IPaymentGatewayHttpClient
	{
		Task<PaymentResponse> RequestBankPayment(PaymentRequest request, string url, CancellationToken cancellationToken);
		Task<PaymentRetrievalResponse> RetrievePaymentDetails(PaymentRetrievalRequest request, string url);
	}

	public class PaymentGatewayHttpClient : IPaymentGatewayHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};

		public PaymentGatewayHttpClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<PaymentResponse> RequestBankPayment(PaymentRequest request, string url, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(url, content, cancellationToken);

			var result = await response.Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<PaymentResponse>(result, _options);
		}
		// TODO: URLs
		public async Task<PaymentRetrievalResponse> RetrievePaymentDetails(PaymentRetrievalRequest request, string url)
		{
			if (request is null)
			{
				throw new ArgumentException(nameof(request));
			}
			var response = await _httpClient.GetStringAsync(url);
			return JsonSerializer.Deserialize<PaymentRetrievalResponse>(response, _options);
		}
	}
}