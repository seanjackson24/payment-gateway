using System;
using Xunit;
using Xunit.Abstractions;
using PaymentGateway.Services;
using ServiceStack.Redis;

namespace PaymentGateway.Tests
{
	public class RedisServiceTests : IDisposable
	{
		private readonly ITestOutputHelper _ouput;
		private const string redisConnectionString = "127.0.0.1:6379";
		private readonly RedisLockActionService _redisService;
		private readonly IRedisClientsManager _pool;

		public RedisServiceTests(ITestOutputHelper output)
		{
			_ouput = output;
			var pool = new RedisManagerPool(redisConnectionString);
			_pool = pool;
			_redisService = new RedisLockActionService(pool);
		}

		public void Dispose()
		{
			_pool.Dispose();
		}

		// [Fact]
		// public void NullOrWhitespace_ThrowsException()
		// {
		// 	Action action = () => _ouput.WriteLine("Success!");
		// 	_redisService.TryExecuteLockAction(action);
		// }

		// [Fact]
		// public async Task NullOrWhitespace_ThrowsException2()
		// {
		// 	var tasks = new List<Task>();
		// 	for (int i = 0; i < 5; i++)
		// 	{
		// 		tasks.Add(Task.Factory.StartNew(() =>
		// 		{
		// 			Action action = () =>
		// 							{
		// 								Thread.Sleep(2000);
		// 								_ouput.WriteLine("Success!");
		// 							};
		// 			// TODO: consider API design for this function
		// 			_redisService.TryAcquireLock("test3", action);
		// 		}));
		// 	}

		// 	Task.WaitAll(tasks.ToArray());
		// }
	}
}
