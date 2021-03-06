﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using PaymentGateway.Common.Models;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Services;

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

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<PaymentResponse>> Put([FromBody] PaymentRequest request, CancellationToken cancellationToken)
		{
			try
			{
				return await _paymentService.PerformPayment(request, cancellationToken);
			}
			catch (PaymentAlreadyExistsException)
			{
				_logger.LogWarning("A payment with ID {paymentId} already exists", request.PaymentId);
				var modelState = new ModelStateDictionary();
				modelState.AddModelError(nameof(request.PaymentId), "A payment with this unique ID already exists");
				return Conflict(modelState);
			}
		}
	}
}
