using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentRetrievalController : ControllerBase
	{
		private readonly ILogger<PaymentRetrievalController> _logger;

		public PaymentRetrievalController(ILogger<PaymentRetrievalController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public PaymentRetrievalResponse Post([FromBody] PaymentRetrievalRequest request)
		{

			throw new NotImplementedException();
		}
	}
}