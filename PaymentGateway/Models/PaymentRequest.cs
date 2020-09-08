using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
	// TODO: test private setters 
	public class PaymentRequest
	{
		public PaymentRequest(string paymentId, string cardNumber, string expiryDate, string cvv, string currencyCode, decimal paymentamount)
		{
			this.PaymentId = paymentId;
			this.CardNumber = cardNumber;
			this.ExpiryDate = expiryDate;
			this.CVV = cvv;
			this.CurrencyCode = currencyCode;
			this.PaymentAmount = paymentamount;
		}

		// validate model
		/// <summary>
		/// A unique ID for this payment, to prevent duplicates
		/// </summary>
		[Required]
		public string PaymentId { get; set; }

		[MaxLength(16)]
		[MinLength(16)]
		[CreditCard]
		public string CardNumber { get; private set; }

		[MinLength(4)]
		[MaxLength(4)]
		public string ExpiryDate { get; private set; }

		[MinLength(3)]
		[MaxLength(3)]
		public string CVV { get; private set; }

		// TODO: Consider amount in dollars vs cents
		[System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
		public decimal PaymentAmount { get; set; }

		// TODO: enum for currency codes?
		public string CurrencyCode { get; private set; }
	}
}
