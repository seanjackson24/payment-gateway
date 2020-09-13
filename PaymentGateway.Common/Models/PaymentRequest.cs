using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Common.Models
{
	public class PaymentRequest
	{
		public PaymentRequest(string paymentId, string cardNumber, string expiryDate, string cvv, string currencyCode, int paymentAmountInCents)
		{
			PaymentId = paymentId;
			CardNumber = cardNumber;
			ExpiryDate = expiryDate;
			CVV = cvv;
			CurrencyCode = currencyCode;
			PaymentAmountInCents = paymentAmountInCents;
		}

		/// <summary>
		/// A unique ID for this payment, to prevent duplicates
		/// </summary>
		[Required]
		public string PaymentId { get; }

		[Required]
		[CreditCard]
		public string CardNumber { get; }

		[Required]
		[StringLength(4, MinimumLength = 4)]
		public string ExpiryDate { get; }

		[Required]
		[StringLength(3, MinimumLength = 3)]
		public string CVV { get; }

		[Required]
		[Range(1, int.MaxValue)]
		public int PaymentAmountInCents { get; }

		[Required]
		[EnumDataType(typeof(Iso4217CurrencyCode))]
		public string CurrencyCode { get; }
	}
}
