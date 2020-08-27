using System;
using System.Collections.Generic;
using System.Linq;
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
		public PaymentResponse Post([FromBody] PaymentRequest request)
		{
			if (!ModelState.IsValid)
			{

			}

			throw new NotImplementedException();
		}
	}
}
