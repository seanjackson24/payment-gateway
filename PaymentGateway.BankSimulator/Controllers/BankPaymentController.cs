using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.BankSimulator.Models;

namespace PaymentGateway.BankSimulator.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BankPaymentController : ControllerBase
	{
		private const string AcceptedCardNumber = "4111111111111111";
		private const string DeclinedCardNumber = "4111111111111112";
		private readonly ILogger<BankPaymentController> _logger;

		public BankPaymentController(ILogger<BankPaymentController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public async Task<BankResponseModel> Post([FromBody] BankRequestModel request)
		{
			switch (request.CardNumber)
			{
				case AcceptedCardNumber:
					return BankResponseModel.Accepted;
				case DeclinedCardNumber:
					return BankResponseModel.Declined;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
