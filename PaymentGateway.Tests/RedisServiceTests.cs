using System;
using PaymentGateway.Domain.Services;
using Xunit.Abstractions;
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
			_pool = new RedisManagerPool(redisConnectionString);
			_redisService = new RedisLockActionService(new RedisManagerPool(redisConnectionString));
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
		// 			_redisService.TryAcquireLock("test3", action);
		// 		}));
		// 	}

		// 	Task.WaitAll(tasks.ToArray());
		// }
	}
}
