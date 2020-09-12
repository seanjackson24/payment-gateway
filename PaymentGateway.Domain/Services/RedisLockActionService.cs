using System;
using System.Threading.Tasks;
using PaymentGateway.Domain.DomainModels;
using ServiceStack.Redis;

namespace PaymentGateway.Domain.Services
{
	public class RedisLockActionService : ILockActionService
	{
		private readonly IRedisClientsManager _redisClientManager;

		public RedisLockActionService(IRedisClientsManager redisClientManager)
		{
			_redisClientManager = redisClientManager;
		}

		public async Task<LockActionResult<TResult>> TryExecuteLockAction<TResult>(LockAction<TResult> lockAction)
		{
			if (lockAction is null)
			{
				throw new ArgumentNullException(nameof(lockAction));
			}

			if (string.IsNullOrWhiteSpace(lockAction.UniqueIdentifier))
			{
				throw new ArgumentException("Lock Action unique identifier cannot be null or whitespace", nameof(lockAction));
			}

			try
			{
				using (IRedisClient redisClient = _redisClientManager.GetClient())
				{
					// using (redisClient.AcquireLock(lockAction.UniqueIdentifier, lockAction.MaxAge))
					using (redisClient.AcquireEnhancedLock(lockAction.UniqueIdentifier, lockAction.Timeout, lockAction.MaxAge))
					{
						var result = await lockAction.Action().ConfigureAwait(false);
						return new LockActionResult<TResult>(lockAction.UniqueIdentifier, result);
					}
				}
			}
			catch (TimeoutException)
			{
				return new LockActionResult<TResult>(lockAction.UniqueIdentifier);
			}
		}
	}
}