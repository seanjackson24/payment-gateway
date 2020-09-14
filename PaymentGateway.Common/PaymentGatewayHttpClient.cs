using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Common.Models;

namespace PaymentGateway.Common
{
	public interface IPaymentGatewayHttpClient
	{
		Task<PaymentResponse> RequestBankPayment(PaymentRequest request, CancellationToken cancellationToken);
		Task<PaymentRetrievalResponse> RetrievePaymentDetails(PaymentRetrievalRequest request);
	}

	public class PaymentGatewayHttpClient : IPaymentGatewayHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		private readonly IConfiguration _configuration;

		public PaymentGatewayHttpClient(HttpClient httpClient, IConfiguration configuration)
		{
			_options.Converters.Add(new JsonStringEnumConverter());
			_httpClient = httpClient;
			_configuration = configuration;
		}

		public async Task<PaymentResponse> RequestBankPayment(PaymentRequest request, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}
			var baseUri = GetBaseUri();
			var url = new Uri(baseUri, Urls.RequestBankPayment);

			var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(url, content, cancellationToken);
			try
			{
				response.EnsureSuccessStatusCode();
				var result = await response.Content.ReadAsStringAsync();

				return JsonSerializer.Deserialize<PaymentResponse>(result, _options);
			} 
			catch (HttpRequestException ex)
            {
				ex.Data.Add("StatusCode", response.StatusCode);
				var problem = await response.Content.ReadAsStringAsync();
				ex.Data.Add("ProblemDetails", JsonSerializer.Deserialize<ProblemDetails>(problem));
				throw;
            }
		}

		public async Task<PaymentRetrievalResponse> RetrievePaymentDetails(PaymentRetrievalRequest request)
		{
			if (request is null)
			{
				throw new ArgumentException(nameof(request));
			}
			var baseUri = GetBaseUri();
			var url = new Uri(baseUri, Urls.RetrievePaymentDetails);
			var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
			var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
			httpRequest.Content = content;
			var response = await _httpClient.SendAsync(httpRequest);
			try
			{
				response.EnsureSuccessStatusCode();

				var result = await response.Content.ReadAsStringAsync();

				return JsonSerializer.Deserialize<PaymentRetrievalResponse>(result, _options);
			}
			catch (HttpRequestException ex)
            {
				ex.Data.Add("StatusCode", response.StatusCode);
				var problem = await response.Content.ReadAsStringAsync();
				ex.Data.Add("ProblemDetails", JsonSerializer.Deserialize<ProblemDetails>(problem));
				throw;
			}
		}

		private Uri GetBaseUri()
		{
			return new Uri(_configuration.GetValue<string>("PaymentGatewayRootUrl"));
		}
	}

	internal static class Urls
	{
		internal const string RequestBankPayment = "Payment";
		internal const string RetrievePaymentDetails = "PaymentRetrieval";
	}
}