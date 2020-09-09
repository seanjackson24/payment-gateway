using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentRetrievalController : ControllerBase
	{
		private readonly ILogger<PaymentRetrievalController> _logger;
		private readonly IPaymentRetrievalService _paymentRetrievalService;

		public PaymentRetrievalController(ILogger<PaymentRetrievalController> logger, IPaymentRetrievalService paymentRetrievalService)
		{
			_logger = logger;
			_paymentRetrievalService = paymentRetrievalService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<PaymentRetrievalResponse>> Index([FromBody] PaymentRetrievalRequest request)
		{
			var result = await _paymentRetrievalService.GetPaymentInformation(request.PaymentId);
			if (result == null)
			{
				return NotFound();
			}
			return result;
		}
	}
}