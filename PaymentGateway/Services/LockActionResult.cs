namespace PaymentGateway.Services
{
	public class LockActionResult<TResult>
	{
		public string ActionIdentifier { get; private set; }
		public TResult ActionResult { get; private set; }

		public bool WasSuccessful { get; private set; }

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