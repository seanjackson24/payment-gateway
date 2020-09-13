using Microsoft.AspNetCore.Mvc;
using PaymentGateway.BankSimulator.Models;

namespace PaymentGateway.BankSimulator.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BankPaymentController : ControllerBase
	{
		private const string AcceptedCardNumber = "4111111111111111";

		[HttpPost]
		public BankResponseModel Post([FromBody] BankRequestModel request)
		{
			switch (request.CardNumber)
			{
				case AcceptedCardNumber:
					return BankResponseModel.Accepted();
				default:
					return BankResponseModel.Declined();
			}
		}
	}
}
