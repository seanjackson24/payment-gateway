using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
	public class PaymentRequest
	{
		public PaymentRequest(string paymentId, string cardNumber, string expiryDate, string cvv, string currencyCode, decimal paymentamount)
		{
			this.PaymentId = paymentId;
			this.CardNumber = cardNumber;
			this.ExpiryDate = expiryDate;
			this.CVV = cvv;
			this.CurrencyCode = currencyCode;
			this.PaymentAmountInCents = paymentamount;
		}

		/// <summary>
		/// A unique ID for this payment, to prevent duplicates
		/// </summary>
		[Required]
		public string PaymentId { get; private set; }

		[Required]
		[CreditCard]
		public string CardNumber { get; private set; }

		[Required]
		[StringLength(4, MinimumLength = 4)]
		public string ExpiryDate { get; private set; }

		[Required]
		[StringLength(3, MinimumLength = 3)]
		public string CVV { get; private set; }

		[Required]
		[Range(1, int.MaxValue)]
		public decimal PaymentAmountInCents { get; private set; }

		[Required]
		[EnumDataType(typeof(Iso4217CurrencyCode))]
		public string CurrencyCode { get; private set; }
	}
}
