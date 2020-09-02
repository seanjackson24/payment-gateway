using System.Threading.Tasks;

namespace PaymentGateway.Services
{
	public interface ILockActionService
	{
		Task<LockActionResult<TResult>> TryExecuteLockAction<TResult>(LockAction<TResult> lockAction);
	}
}