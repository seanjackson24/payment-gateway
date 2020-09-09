using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Controllers
{
	[ApiController]
	public class ErrorController : ControllerBase
	{
		[Route("/error")]
		public IActionResult Error() => Problem();
	}
}