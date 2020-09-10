using System;
using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
	[Serializable]
	public class PaymentAlreadyExistsException : Exception
	{
		public PaymentAlreadyExistsException()
		{
		}

		public PaymentAlreadyExistsException(string message) : base(message)
		{
		}

		public PaymentAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected PaymentAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}