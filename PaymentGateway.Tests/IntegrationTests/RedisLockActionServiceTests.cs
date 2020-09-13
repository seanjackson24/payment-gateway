using System;
using System.Threading.Tasks;
using PaymentGateway.Domain.DomainModels;
using PaymentGateway.Domain.Services;
using ServiceStack;
using ServiceStack.Redis;
using Xunit;

namespace PaymentGateway.Tests.IntegrationTests
{
	public class RedisLockActionServiceTests
	{
		private readonly RedisLockActionService _service;
		private const string RedisConnectionString = "127.0.0.1:6379";

		public RedisLockActionServiceTests()
		{
			IRedisClientsManager pool = new RedisManagerPool(RedisConnectionString);
			_service = new RedisLockActionService(pool);
		}


		[Fact]
		public async Task TryExecuteLockAction_ActionIsNull_ThrowsException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.TryExecuteLockAction<string>(null));
		}

		[Fact]
		public async Task TryExecuteLockAction_ActionIdentifierIsNull_ThrowsException()
		{
			async Task<string> fn()
			{
				return await Task.FromResult("");
			}


			TimeSpan timeout = TimeSpan.FromSeconds(5);
			TimeSpan maxage = TimeSpan.FromSeconds(5);
			var action = new LockAction<string>(null, timeout, maxage, fn);
			await Assert.ThrowsAsync<ArgumentException>(async () => await _service.TryExecuteLockAction<string>(action));
		}


		[Fact]
		public async Task TryExecuteLockAction_SuccessfullyObtainsLock()
		{
			const int result = 5;

			async Task<int> fn()
			{
				return await Task.FromResult(result);
			}
			string id = Guid.NewGuid().ToString();
			TimeSpan timeout = TimeSpan.FromSeconds(5);
			TimeSpan maxage = TimeSpan.FromSeconds(5);
			var action = new LockAction<int>(id, timeout, maxage, fn);
			var lockResult = await _service.TryExecuteLockAction(action);
			Assert.NotNull(lockResult);
			Assert.Equal(id, lockResult.ActionIdentifier);
			Assert.Equal(result, lockResult.ActionResult);
			Assert.True(lockResult.WasSuccessful);
		}


		[Fact]
		public async Task TryExecuteLockAction_TwoDifferentLocks_SuccesfullyObtainsLock()
		{
			const int result = 5;
			const int result2 = 6;

			async Task<int> fn()
			{
				return await Task.FromResult(result);
			}
			async Task<int> fn2()
			{
				return await Task.FromResult(result2);
			}
			string id = Guid.NewGuid().ToString();
			string id2 = Guid.NewGuid().ToString();
			TimeSpan timeout = TimeSpan.FromSeconds(5);
			TimeSpan maxage = TimeSpan.FromSeconds(5);
			var action = new LockAction<int>(id, timeout, maxage, fn);
			var action2 = new LockAction<int>(id2, timeout, maxage, fn2);
			var lockResult = await _service.TryExecuteLockAction(action);
			var lockResult2 = await _service.TryExecuteLockAction(action2);

			Assert.Equal(id2, lockResult2.ActionIdentifier);
			Assert.Equal(result2, lockResult2.ActionResult);
			Assert.True(lockResult2.WasSuccessful);
		}

		[Fact]
		public async Task TryExecuteLockAction_SameLockAfterOriginalReleased_SuccessfullyObtainsLock()
		{
			const int result = 5;

			async Task<int> fn()
			{
				return await Task.FromResult(result);
			}

			string id = Guid.NewGuid().ToString();
			TimeSpan timeout = TimeSpan.FromSeconds(5);
			TimeSpan maxage = TimeSpan.FromSeconds(5);
			var action = new LockAction<int>(id, timeout, maxage, fn);
			var lockResult = await _service.TryExecuteLockAction(action);
			var lockResult2 = await _service.TryExecuteLockAction(action);

			Assert.True(lockResult2.WasSuccessful);
			Assert.True(lockResult.WasSuccessful);

			Assert.Equal(id, lockResult.ActionIdentifier);
			Assert.Equal(id, lockResult2.ActionIdentifier);

			Assert.Equal(result, lockResult.ActionResult);
			Assert.Equal(result, lockResult2.ActionResult);
		}

		[Fact]
		public void TryExecureLockAction_TwoConcurrentLocks_WithLongExecutionTime_ShortAcquireTime_ReturnsUnsuccessfulForOne()
		{
			const int result = 5;
			const int result2 = 6;
			string id = Guid.NewGuid().ToString();

			async Task<int> fn()
			{
				await Task.Delay(5000);
				return await Task.FromResult(result);
			}
			async Task<int> fn2()
			{
				await Task.Delay(5000);
				return await Task.FromResult(result2);
			}

			TimeSpan timeout = TimeSpan.FromSeconds(1);
			TimeSpan maxage = TimeSpan.FromSeconds(10);

			var action = new LockAction<int>(id, timeout, maxage, fn);
			var action2 = new LockAction<int>(id, timeout, maxage, fn2);
			Task<LockActionResult<int>> t1 = _service.TryExecuteLockAction(action);
			Task<LockActionResult<int>> t2 = _service.TryExecuteLockAction(action2);

			Task.WaitAll(t1, t2);

			var lockResult = t1.GetResult();
			var lockResult2 = t2.GetResult();

			Assert.True(lockResult.WasSuccessful);
			Assert.False(lockResult2.WasSuccessful);
			Assert.Equal(id, lockResult2.ActionIdentifier);
		}

		[Fact]
		public void TryExecuteLockAction_ShortMaxAge_AcquiresLock()
		{
			const int result = 5;
			const int result2 = 6;
			string id = Guid.NewGuid().ToString();

			async Task<int> fn()
			{
				await Task.Delay(5000);
				return await Task.FromResult(result);
			}
			async Task<int> fn2()
			{
				await Task.Delay(5000);
				return await Task.FromResult(result2);
			}

			TimeSpan timeout = TimeSpan.FromSeconds(10);
			TimeSpan maxage = TimeSpan.FromSeconds(1);

			var action = new LockAction<int>(id, timeout, maxage, fn);
			var action2 = new LockAction<int>(id, timeout, maxage, fn2);
			Task<LockActionResult<int>> t1 = _service.TryExecuteLockAction(action);
			Task<LockActionResult<int>> t2 = _service.TryExecuteLockAction(action2);

			Task.WaitAll(t1, t2);

			var lockResult = t1.GetResult();
			var lockResult2 = t2.GetResult();

			Assert.True(lockResult.WasSuccessful);
			Assert.True(lockResult2.WasSuccessful);
			Assert.Equal(id, lockResult2.ActionIdentifier);
			Assert.Equal(id, lockResult.ActionIdentifier);

			Assert.Equal(result, lockResult.ActionResult);
			Assert.Equal(result2, lockResult2.ActionResult);
		}

		[Fact]
		public void TryExecuteLockAction_ShortTimeout_DoesNotAcquireLock()
		{
			const int result = 5;
			string id = Guid.NewGuid().ToString();

			async Task<int> fn()
			{
				await Task.Delay(5000);
				return await Task.FromResult(result);
			}

			TimeSpan timeout = TimeSpan.FromTicks(1);
			TimeSpan maxage = TimeSpan.FromSeconds(1);

			var action = new LockAction<int>(id, timeout, maxage, fn);
			Task<LockActionResult<int>> t1 = _service.TryExecuteLockAction(action);

			var lockResult = t1.GetResult();

			Assert.False(lockResult.WasSuccessful);
			Assert.Equal(id, lockResult.ActionIdentifier);
		}

	}
}
