using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly ILogger<PaymentController> _logger;

		public PaymentController(ILogger<PaymentController> logger)
		{
			_logger = logger;
		}

		//TODO: post or put?
		[HttpPost]
		public async Task<PaymentResponse> Post([FromBody] PaymentRequest request)
		{
			if (!ModelState.IsValid)
			{

			}
			// _paymentService.PerformPayment(request);
			throw new NotImplementedException();
		}
	}
}
