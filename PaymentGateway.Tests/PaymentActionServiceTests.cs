using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.DomainModels;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Services;
using Xunit;

namespace PaymentGateway.Tests
{
	public class PaymentActionServiceTests
	{
		private const string CardNumber = "1234567890123456";
		private readonly PaymentActionService _service;
		private readonly Mock<IPaymentRepository> _paymentRespository;
		private readonly Mock<ICardMaskingService> _cardMaskingService;
		private readonly Mock<ITimeProvider> _timeProvider;
		private readonly Mock<IBankService> _bankService;
		private readonly Mock<ILogger> _logger;

		public PaymentActionServiceTests()
		{
			_paymentRespository = new Mock<IPaymentRepository>();
			_cardMaskingService = new Mock<ICardMaskingService>();
			_timeProvider = new Mock<ITimeProvider>();
			_bankService = new Mock<IBankService>();
			_logger = new Mock<ILogger>();
			_service = new PaymentActionService(_paymentRespository.Object, _cardMaskingService.Object, _timeProvider.Object, _bankService.Object, _logger.Object);
		}

		[Fact]
		public async Task PerformPayment_PaymentExists_ThrowsExceptionAsync()
		{
			string paymentId = Guid.NewGuid().ToString();
			_paymentRespository.Setup(x => x.PaymentExists(paymentId, It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

			var request = new PaymentRequest(paymentId, "1234", "5678", "910", "GBP", 30);
			await Assert.ThrowsAsync<PaymentAlreadyExistsException>(async () => await _service.PerformPayment(request, CancellationToken.None));
		}

		[Fact]
		public async Task PerformPayment_BankResponseAccepted_ReturnsAcceptedResult()
		{
			string paymentId = Guid.NewGuid().ToString();
			var request = new PaymentRequest(paymentId, "1234", "5678", "910", "GBP", 30);
			_paymentRespository.Setup(x => x.PaymentExists(paymentId, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));
			_bankService.Setup(x => x.PerformPaymentAsync(request)).Returns(Task.FromResult(new BankResponse() { Status = PaymentStatus.Accepted }));

			var result = await _service.PerformPayment(request, CancellationToken.None);
			Assert.Equal(true, result.WasPaymentAccepted);
		}

		[Fact]
		public async Task PerformPayment_BankResponseDeclined_ReturnsDeclinedResult()
		{
			string paymentId = Guid.NewGuid().ToString();
			var request = new PaymentRequest(paymentId, "1234", "5678", "910", "GBP", 30);
			_paymentRespository.Setup(x => x.PaymentExists(paymentId, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));
			_bankService.Setup(x => x.PerformPaymentAsync(request)).Returns(Task.FromResult(new BankResponse() { Status = PaymentStatus.Declined }));

			var result = await _service.PerformPayment(request, CancellationToken.None);
			Assert.Equal(false, result.WasPaymentAccepted);
		}

		[Fact]
		public async Task PerformPayment_CardNumberIsMasked()
		{
			string paymentId = Guid.NewGuid().ToString();
			var request = new PaymentRequest(paymentId, CardNumber, "5678", "910", "GBP", 30);
			_paymentRespository.Setup(x => x.PaymentExists(paymentId, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));
			_bankService.Setup(x => x.PerformPaymentAsync(request)).Returns(Task.FromResult(new BankResponse() { Status = PaymentStatus.Declined }));

			var result = await _service.PerformPayment(request, CancellationToken.None);
			_cardMaskingService.Verify(x => x.MaskCardNumber(CardNumber));
		}
	}
}



