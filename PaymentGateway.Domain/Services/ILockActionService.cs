using System.Threading.Tasks;
using PaymentGateway.Domain.DomainModels;

namespace PaymentGateway.Domain.Services
{
	public interface ILockActionService
	{
		Task<LockActionResult<TResult>> TryExecuteLockAction<TResult>(LockAction<TResult> lockAction);
	}
}