using System;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.BankSimulator.Models;

namespace PaymentGateway.BankSimulator.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BankPaymentController : ControllerBase
	{
		private const string AcceptedCardNumber = "4111111111111111";
		private const string DeclinedCardNumber = "4111111111111112";

		[HttpPost]
		public BankResponseModel Post([FromBody] BankRequestModel request)
		{
			switch (request.CardNumber)
			{
				case AcceptedCardNumber:
					return BankResponseModel.Accepted();
				case DeclinedCardNumber:
					return BankResponseModel.Declined();
				default:
					throw new NotImplementedException();
			}
		}
	}
}
