using System;
using ServiceStack.Redis;

namespace PaymentGateway.Services
{
	public interface IRedisService
	{
		void AcquireLock(string paymentId, Action lockAction);
	}
	public class RedisService : IRedisService
	{
		private readonly IRedisClientsManager _redisClientManager;

		public RedisService(IRedisClientsManager redisClientManager)
		{
			_redisClientManager = redisClientManager;
		}

		public void AcquireLock(string paymentId, Action lockAction)
		{
			TimeSpan timeOut = TimeSpan.FromSeconds(1);
			using (IRedisClient redisClient = _redisClientManager.GetClient())
			{
				using (redisClient.AcquireLock(paymentId, timeOut))
				{
					lockAction();
				}
			}
		}
	}
}