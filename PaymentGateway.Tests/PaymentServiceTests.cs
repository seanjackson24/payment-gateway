using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.DomainModels;
using PaymentGateway.Domain.Factories;
using PaymentGateway.Domain.Services;
using Xunit;

namespace PaymentGateway.Tests
{
	public class PaymentServiceTests
	{
		private readonly PaymentService _service;
		private readonly Mock<ILockActionService> _lockActionService;
		private readonly Mock<IPaymentResponseFactory> _paymentResponseFactory;
		private readonly Mock<IPaymentActionService> _paymentActionService;

		public PaymentServiceTests()
		{
			var config = new ConfigurationBuilder().AddInMemoryCollection().Build();

			_lockActionService = new Mock<ILockActionService>();
			_paymentResponseFactory = new Mock<IPaymentResponseFactory>();
			_paymentActionService = new Mock<IPaymentActionService>();
			_service = new PaymentService(_lockActionService.Object, config, _paymentResponseFactory.Object, _paymentActionService.Object);
		}

		[Fact]
		public async Task PerformPayment_CouldNotObtainLock_ReturnsProcessingAsync()
		{
			var paymentId = Guid.NewGuid().ToString();
			var result = new LockActionResult<PaymentActionResult>(paymentId);
			_lockActionService.Setup(x => x.TryExecuteLockAction(It.IsAny<LockAction<PaymentActionResult>>())).Returns(Task.FromResult(result));
			var request = new PaymentRequest(paymentId, "123456", "688", "897", "GBP", 32);
			var ret = await _service.PerformPayment(request, CancellationToken.None);
			_paymentResponseFactory.Verify(x => x.CreateProcessingResponse(paymentId));
		}

		[Fact]
		public async Task PerformPayment_AbleToObtainLock_ReturnsObjectAsync()
		{
			var paymentId = Guid.NewGuid().ToString();
			PaymentActionResult actionResult = new PaymentActionResult(true);
			var result = new LockActionResult<PaymentActionResult>(paymentId, actionResult);
			_lockActionService.Setup(x => x.TryExecuteLockAction(It.IsAny<LockAction<PaymentActionResult>>())).Returns(Task.FromResult(result));
			var request = new PaymentRequest(paymentId, "123456", "688", "897", "GBP", 32);
			var ret = await _service.PerformPayment(request, CancellationToken.None);
			_paymentResponseFactory.Verify(x => x.CreateResponse(paymentId, actionResult));
		}
	}
}