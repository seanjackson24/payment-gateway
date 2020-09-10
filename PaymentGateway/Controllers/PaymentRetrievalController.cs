using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.Services;

namespace PaymentGateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentRetrievalController : ControllerBase
	{
		private readonly IPaymentRetrievalService _paymentRetrievalService;

		public PaymentRetrievalController(IPaymentRetrievalService paymentRetrievalService)
		{
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