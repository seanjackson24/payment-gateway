using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
	public class PaymentRequest
	{
		// validate model
		/// <summary>
		/// A unique ID for this payment, to prevent duplicates
		/// </summary>
		[Required]
		public string PaymentId { get; set; }

		[MaxLength(16)]
		[MinLength(16)]
		public string CardNumber { get; set; }

		[MinLength(4)]
		[MaxLength(4)]
		public string ExpiryDate { get; set; }

		[MinLength(3)]
		[MaxLength(3)]
		public string CVV { get; set; }

		// TODO: Consider amount in dollars vs cents
		public decimal PaymentAmount { get; set; }

		// TODO: enum for currency codes?
		public string CurrencyCode { get; set; }
	}
}
