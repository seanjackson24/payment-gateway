using System;
using System.ComponentModel.DataAnnotations.Schema;
using PaymentGateway.Common.Models;

namespace PaymentGateway.Domain.DomainModels
{
	[Table("Payment")]
	public class Payment
	{
		public string PaymentId { get; set; }
		public string MaskedCardNumber { get; set; }
		public int PaymentAmountInCents { get; set; }
		public DateTime TimestampUtc { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public string BankReference { get; set; }
		public string CardExpiryDate { get; set; }
		public string CurrencyCode { get; set; }
	}
}