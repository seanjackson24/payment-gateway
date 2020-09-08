using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly ILogger<PaymentController> _logger;
		private readonly IPaymentService _paymentService;

		public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
		{
			_logger = logger;
			_paymentService = paymentService;
		}

		//TODO: post or put?
		[HttpPut]
		public async Task<PaymentResponse> Put([FromBody] PaymentRequest request, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid)
			{
				// TODO
				await Task.Delay(1);
			}
			return await _paymentService.PerformPayment(request, cancellationToken);
		}
	}
}
