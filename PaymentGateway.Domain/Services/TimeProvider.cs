using System;

namespace PaymentGateway.Domain.Services
{
	public interface ITimeProvider
	{
		DateTime UtcNow();
	}

	public class TimeProvider : ITimeProvider
	{
		public DateTime UtcNow()
		{
			return DateTime.UtcNow;
		}
	}
}