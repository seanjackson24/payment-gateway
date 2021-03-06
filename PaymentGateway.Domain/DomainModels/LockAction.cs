using System;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DomainModels
{
	public class LockAction<TResult>
	{
		public string UniqueIdentifier { get; }

		public Func<Task<TResult>> Action { get; }

		/// <summary>
		/// The amount of time a client should spend trying to acquire a lock before timing out
		/// </summary>
		public TimeSpan Timeout { get; }

		/// <summary>
		/// The maximum amount of time allowed for a client to execute its <see cref="Action" />. Assume a crashed client
		/// will not be able to recover its lock
		/// </summary>
		public TimeSpan MaxAge { get; }

		public LockAction(string uniqueIdentifier, TimeSpan timeout, TimeSpan maxAge, Func<Task<TResult>> action)
		{
			UniqueIdentifier = uniqueIdentifier;
			Timeout = timeout;
			MaxAge = maxAge;
			Action = action;
		}
	}
}