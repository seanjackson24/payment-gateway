namespace PaymentGateway.Domain.DomainModels
{
	public class LockActionResult<TResult>
	{
		public string ActionIdentifier { get; }
		public TResult ActionResult { get; }

		/// <summary>
		/// Whether or not the Lock Action was able to execute. Returns false if another Thread, Process, or Machine was holding on to the lock
		/// </summary>
		public bool WasSuccessful { get; }

		/// <summary>
		/// Creates an instance of LockActionResult which was unable to execute as another Thread, Process, or Machine was holding on to the lock
		///</summary>
		public LockActionResult(string actionIdentifier)
		{
			WasSuccessful = false;
			ActionIdentifier = actionIdentifier;
		}

		public LockActionResult(string actionIdentifier, TResult actionResult)
		{
			WasSuccessful = true;
			ActionIdentifier = actionIdentifier;
			ActionResult = actionResult;
		}
	}
}