using System;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Models
{
	public class PaymentResponse
	{
		/// <summary>
		/// A unique ID for this payment, to prevent duplicates
		/// </summary>
		public string PaymentId { get; set; }
		public string Status { get; set; }

		public ProblemDetails ProblemDetails { get; set; }
	}
}
