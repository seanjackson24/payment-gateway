using System;
using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
	[Serializable]
	internal class BankNotFoundException : Exception
	{
		public BankNotFoundException()
		{
		}

		public BankNotFoundException(string message) : base(message)
		{
		}

		public BankNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected BankNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}