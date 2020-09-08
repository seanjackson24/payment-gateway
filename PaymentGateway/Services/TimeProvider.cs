using System;

namespace PaymentGateway.Services
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