using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
	public class LockAction<TResult>
	{
		public string UniqueIdentifier { get; private set; }

		// TODO: should we force it to be a task?
		public Func<Task<TResult>> Action { get; private set; }

		public TimeSpan Timeout { get; private set; }

		public LockAction(string uniqueIdentifier, TimeSpan timeout, Func<Task<TResult>> action)
		{
			UniqueIdentifier = uniqueIdentifier;
			Timeout = timeout;
			Action = action;
		}
	}
}