using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Common;
using PaymentGateway.Common.Models;
using Xunit;
using Xunit.Abstractions;

namespace PaymentGateway.Tests.IntegrationTests
{
	public class EndToEndTests : EndToEndTestBase
	{
		private const string acceptedCard = "4111111111111111";
		private const string DeclinedCardNumber = "5105105105105100";
		private readonly IConfigurationRoot _configuration;

		public EndToEndTests()
		{
			StartAll();

			var initialData = new List<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("PaymentGatewayRootUrl", "http://localhost:5001")
			};
			_configuration = new ConfigurationBuilder().AddInMemoryCollection(initialData).Build();
		}

		[Fact]
		public async Task AcceptedCard()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", "gbp", 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);
				Assert.Equal(id, result.PaymentId);
				Assert.Equal(PaymentStatus.Accepted, result.Status);
			}
		}

		[Fact]
		public async Task DeclinedCard()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, DeclinedCardNumber, "0222", "123", "gbp", 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);
				Assert.Equal(id, result.PaymentId);
				Assert.Equal(PaymentStatus.Accepted, result.Status);
			}
		}

		[Fact]
		public async Task MakeSuccessfulPayment_RetrieveDetails_Accepted()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", "gbp", 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);

				// TODO: constructor
				var retrieval = new PaymentRetrievalRequest() { PaymentId = id };
				var retrievalResult = await client.RetrievePaymentDetails(retrieval);
				Assert.Equal(PaymentStatus.Accepted.ToString(), retrievalResult.PaymentStatus);
				Assert.Equal("411111******1111", retrievalResult.MaskedCardNumber);

			}
		}

		[Fact]
		public async Task MakeDeclinedPayment_RetrievalDetails_Declined()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, DeclinedCardNumber, "0222", "123", "gbp", 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);

				var retrieval = new PaymentRetrievalRequest() { PaymentId = id };
				var retrievalResult = await client.RetrievePaymentDetails(retrieval);
				Assert.Equal(PaymentStatus.Declined.ToString(), retrievalResult.PaymentStatus);
				Assert.Equal("510510******5100", retrievalResult.MaskedCardNumber);
			}
		}

		[Fact]
		public async Task RetrievePayment_NotFound_RespondsWithNotFound()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();

				var retrieval = new PaymentRetrievalRequest() { PaymentId = id };

				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RetrievePaymentDetails(retrieval));
				var innerException = exception.InnerException as WebException;
				Assert.NotNull(innerException);
				Assert.Equal(WebExceptionStatus.NameResolutionFailure, innerException.Status);
			}
		}


		// unhappy path: request same payment after completed
		// unhappy path: request negative payment
		// unhappy path: request not credit card number
		// unhappy path: CVV validation
		// unhappy path: expiry validation
		// unhappy path: unknown currency code
		// unhappy path: no payment ID
		// unhappy path: request two payments at once
	}
}