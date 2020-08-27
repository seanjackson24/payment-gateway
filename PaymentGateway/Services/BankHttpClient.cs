using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
	public class BankHttpClient
	{
		private readonly HttpClient _httpClient;

		public BankHttpClient(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task<TResponse> RequestBank<TResponse>(string itemName, CancellationToken cancellationToken)
		{
			// TODO: cache
			if (string.IsNullOrWhiteSpace(itemName))
			{
				return default(TResponse);
			}
			string url = "TODO";
			var requestUrl = new Uri(url + itemName.ToLowerInvariant());
			var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
			var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

			using (var contentStream = await response.Content.ReadAsStreamAsync())
			{
				try
				{
					// return await as opposed to just return as we are inside a using statement
					return await JsonSerializer.DeserializeAsync<TResponse>(contentStream, new JsonSerializerOptions(), cancellationToken);
				}
				catch (Exception ex)
				{
					return default(TResponse);
				}
			}
		}
	}
}