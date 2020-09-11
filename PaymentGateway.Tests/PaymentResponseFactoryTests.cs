using System;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.DomainModels;
using PaymentGateway.Domain.Factories;
using Xunit;

namespace PaymentGateway.Tests
{
	public class PaymentResponseFactoryTests
	{
		private readonly PaymentResponseFactory _factory = new PaymentResponseFactory();

		[Fact]
		public void CreateProcessingResponse_ReturnsProcessingObject()
		{
			string paymentId = Guid.NewGuid().ToString();
			var result = _factory.CreateProcessingResponse(paymentId);
			Assert.Equal(paymentId, result.PaymentId);
			Assert.Equal(PaymentStatus.Processing, result.Status);
		}

		[Fact]
		public void CreateResponse_AcceptedPayment_ReturnsAccepted()
		{
			string paymentId = Guid.NewGuid().ToString();
			var actionResult = new PaymentActionResult(true);
			var result = _factory.CreateResponse(paymentId, actionResult);
			Assert.Equal(paymentId, result.PaymentId);
			Assert.Equal(PaymentStatus.Accepted, result.Status);
		}

		[Fact]
		public void CreateResponse_DeclinedPayment_ReturnsDeclined()
		{
			string paymentId = Guid.NewGuid().ToString();
			var actionResult = new PaymentActionResult(false);
			var result = _factory.CreateResponse(paymentId, actionResult);
			Assert.Equal(paymentId, result.PaymentId);
			Assert.Equal(PaymentStatus.Declined, result.Status);
		}
	}
}