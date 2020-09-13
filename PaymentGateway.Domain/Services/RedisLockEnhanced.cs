using System;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace PaymentGateway.Domain.Services
{
	// modified from https://raw.githubusercontent.com/ServiceStack/ServiceStack.Redis/v5.9.2/src/ServiceStack.Redis/RedisLock.cs
	public class RedisLockEnhanced
		: IDisposable
	{
		private readonly IRedisClient _redisClient;
		private readonly string _key;
		public static readonly TimeSpan DefaultAcquisitionTimeOut = TimeSpan.FromSeconds(30);

		/// <summary>
		/// Acquires a distributed lock on the specified key.
		/// </summary>
		/// <param name="redisClient">The client to use to acquire the lock.</param>
		/// <param name="key">The key to acquire the lock on.</param>
		/// <param name="acquisitionTimeOut">The amount of time to wait while trying to acquire the lock. Defaults to <see cref="DefaultAcquisitionTimeOut"/>.</param>
		/// <param name="timeOut">After this amount of time expires, the lock will be invalidated and other clients will be allowed to establish a new lock on the same key. Deafults to 1 year.</param>
		public RedisLockEnhanced(IRedisClient redisClient, string key, TimeSpan? acquisitionTimeOut, TimeSpan? timeOut)
		{
			_redisClient = redisClient;
			_key = key;

			ExecUtils.RetryUntilTrue(
				() =>
					{
						//This pattern is taken from the redis command for SETNX http://redis.io/commands/setnx

						//Calculate a unix time for when the lock should expire
						var realSpan = timeOut ?? new TimeSpan(365, 0, 0, 0); //if nothing is passed in the timeout hold for a year
						var expireTime = DateTime.UtcNow.Add(realSpan);
						var lockString = (expireTime.ToUnixTimeMs() + 1).ToString();

						//Try to set the lock, if it does not exist this will succeed and the lock is obtained
						var nx = redisClient.SetValueIfNotExists(key, lockString);
						if (nx)
							return true;

						//If we've gotten here then a key for the lock is present. This could be because the lock is
						//correctly acquired or it could be because a client that had acquired the lock crashed (or didn't release it properly).
						//Therefore we need to get the value of the lock to see when it should expire

						redisClient.Watch(key);
						var lockExpireString = redisClient.Get<string>(key);
						if (!long.TryParse(lockExpireString, out var lockExpireTime))
						{
							redisClient.UnWatch();  // since the client is scoped externally
							return false;
						}

						//If the expire time is greater than the current time then we can't let the lock go yet
						if (lockExpireTime > DateTime.UtcNow.ToUnixTimeMs())
						{
							redisClient.UnWatch();  // since the client is scoped externally
							return false;
						}

						//If the expire time is less than the current time then it wasn't released properly and we can attempt to 
						//acquire the lock. The above call to Watch(_lockKey) enrolled the key in monitoring, so if it changes
						//before we call Commit() below, the Commit will fail and return false, which means that another thread 
						//was able to acquire the lock before we finished processing.
						using (var trans = redisClient.CreateTransaction()) // we started the "Watch" above; this tx will succeed if the value has not moved 
						{
							trans.QueueCommand(r => r.Set(key, lockString));
							return trans.Commit(); //returns false if Transaction failed
						}
					},
				acquisitionTimeOut ?? DefaultAcquisitionTimeOut // the key difference with StackExchange.Redis's version: we loop for this parameter instead of the timeout parameter
			);
		}


		public void Dispose()
		{
			_redisClient.Remove(_key);
		}
	}

	internal static class RedisClientLockExtensions
	{
		public static IDisposable AcquireEnhancedLock(this IRedisClient client, string key, TimeSpan timeOut, TimeSpan maxAge)
		{
			return new RedisLockEnhanced(client, key, timeOut, maxAge);
		}
	}
}