using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Common;
using PaymentGateway.Common.Models;
using Xunit;

namespace PaymentGateway.Tests.IntegrationTests
{
	[Collection("Non-Parallel Collection")]
	public class EndToEndTests : EndToEndTestBase
	{
		[Fact]
		public async Task AcceptedCard()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, 44);
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
				var request = new PaymentRequest(id, DeclinedCardNumber, "0222", "123", CurrencyCode, 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);
				Assert.Equal(id, result.PaymentId);
				Assert.Equal(PaymentStatus.Declined, result.Status);
			}
		}

		[Fact]
		public async Task MakeSuccessfulPayment_RetrieveDetails_Accepted()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);

				// TODO: constructor
				var retrieval = new PaymentRetrievalRequest(id);
				var retrievalResult = await client.RetrievePaymentDetails(retrieval);
				Assert.Equal(PaymentStatus.Accepted, retrievalResult.PaymentStatus);
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
				var request = new PaymentRequest(id, DeclinedCardNumber, "0222", "123", CurrencyCode, 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);

				var retrieval = new PaymentRetrievalRequest(id);
				var retrievalResult = await client.RetrievePaymentDetails(retrieval);
				Assert.Equal(PaymentStatus.Declined, retrievalResult.PaymentStatus);
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

				var retrieval = new PaymentRetrievalRequest(id);

				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RetrievePaymentDetails(retrieval));
				Assert.Equal(HttpStatusCode.NotFound, exception.Data["StatusCode"]);
			}
		}


		[Fact]
		public async Task RequestPayment_SamePaymentAfterCompleted_ReturnsError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, 44);
				var result = await client.RequestBankPayment(request, CancellationToken.None);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.Conflict, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_NegativeValue_ModelValidationError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, -1);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.BadRequest, exception.Data["StatusCode"]);
			}
		}


		[Fact]
		public async Task RequestPayment_InvalidCreditCardNumber_ModelValidationError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, "not a card", "0222", "123", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.BadRequest, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_InvalidCvv_ModelValidationError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "invalid cvv", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.BadRequest, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_InvalidExpiry_ModelValidationError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "invalid expiry", "123", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.BadRequest, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_InvalidCurrencyCode_ModelValidationError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", ";@~", 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.BadRequest, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_NoPaymentId_ModelValidationError()
		{
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var request = new PaymentRequest("", acceptedCard, "0222", "123", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.BadRequest, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_RedisIsDown_InternalServerError()
		{
			StopRedis();
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.InternalServerError, exception.Data["StatusCode"]);
			}
		}

		[Fact]
		public async Task RequestPayment_BankIsDown_InternalServerError()
		{
			StopBank();
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.InternalServerError, exception.Data["StatusCode"]);
			}
		}
		[Fact]
		public async Task RequestPayment_SqlIsDown_InternalServerError()
		{
			StopDatabase();
			using (var httpClient = new HttpClient())
			{
				var client = new PaymentGatewayHttpClient(httpClient, _configuration);
				var id = Guid.NewGuid().ToString();
				var request = new PaymentRequest(id, acceptedCard, "0222", "123", CurrencyCode, 44);
				var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.RequestBankPayment(request, CancellationToken.None));
				Assert.Equal(HttpStatusCode.InternalServerError, exception.Data["StatusCode"]);
			}
		}

		// unhappy path: request two payments at once
	}
}