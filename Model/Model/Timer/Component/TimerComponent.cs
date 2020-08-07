using System;
using System.Collections.Generic;

namespace NiceET
{
	public class TimerComponent : Entity
	{
		public static TimerComponent Instance { get; set; }

		private readonly Dictionary<long, ITimer> timers = new Dictionary<long, ITimer>();

		/// <summary>
		/// key: time, value: timer id
		/// </summary>
		public readonly MultiMap<long, long> TimeId = new MultiMap<long, long>();

		private readonly Queue<long> timeOutTime = new Queue<long>();

		private readonly Queue<long> timeOutTimerIds = new Queue<long>();

		// 记录最小时间，不用每次都去MultiMap取第一个值
		private long minTime;

		public void Update()
		{
			if (this.TimeId.Count == 0)
			{
				return;
			}

			long timeNow = TimeHelper.Now();

			if (timeNow < this.minTime)
			{
				return;
			}

			foreach (KeyValuePair<long, List<long>> kv in this.TimeId.GetDictionary())
			{
				long k = kv.Key;
				if (k > timeNow)
				{
					minTime = k;
					break;
				}
				this.timeOutTime.Enqueue(k);
			}

			while (this.timeOutTime.Count > 0)
			{
				long time = this.timeOutTime.Dequeue();
				foreach (long timerId in this.TimeId[time])
				{
					this.timeOutTimerIds.Enqueue(timerId);
				}
				this.TimeId.Remove(time);
			}

			while (this.timeOutTimerIds.Count > 0)
			{
				long timerId = this.timeOutTimerIds.Dequeue();
				ITimer timer;
				if (!this.timers.TryGetValue(timerId, out timer))
				{
					continue;
				}

				timer.Run(true);
			}
		}

		public async ETTask<bool> WaitTillAsync(long tillTime, ETCancellationToken cancellationToken)
		{
			if (TimeHelper.Now() > tillTime)
			{
				return true;
			}
			ETTaskCompletionSource<bool> tcs = new ETTaskCompletionSource<bool>();
			OnceWaitTimer timer = EntityFactory.CreateWithParent<OnceWaitTimer, ETTaskCompletionSource<bool>>(this, tcs);
			this.timers[timer.Id] = timer;
			AddToTimeId(tillTime, timer.Id);

			long instanceId = timer.InstanceId;
			cancellationToken.Register(() =>
			{
				if (instanceId != timer.InstanceId)
				{
					return;
				}

				timer.Run(false);

				this.Remove(timer.Id);
			});
			return await tcs.Task;
		}

		public async ETTask<bool> WaitTillAsync(long tillTime)
		{
			if (TimeHelper.Now() > tillTime)
			{
				return true;
			}
			ETTaskCompletionSource<bool> tcs = new ETTaskCompletionSource<bool>();
			OnceWaitTimer timer = EntityFactory.CreateWithParent<OnceWaitTimer, ETTaskCompletionSource<bool>>(this, tcs);
			this.timers[timer.Id] = timer;
			AddToTimeId(tillTime, timer.Id);
			return await tcs.Task;
		}

		public async ETTask<bool> WaitAsync(long time, ETCancellationToken cancellationToken)
		{
			long tillTime = TimeHelper.Now() + time;

			if (TimeHelper.Now() > tillTime)
			{
				return true;
			}

			ETTaskCompletionSource<bool> tcs = new ETTaskCompletionSource<bool>();
			OnceWaitTimer timer = EntityFactory.CreateWithParent<OnceWaitTimer, ETTaskCompletionSource<bool>>(this, tcs);
			this.timers[timer.Id] = timer;
			AddToTimeId(tillTime, timer.Id);
			long instanceId = timer.InstanceId;
			cancellationToken.Register(() =>
			{
				if (instanceId != timer.InstanceId)
				{
					return;
				}

				timer.Run(false);

				this.Remove(timer.Id);
			});
			return await tcs.Task;
		}

		public async ETTask<bool> WaitAsync(long time)
		{
			long tillTime = TimeHelper.Now() + time;
			ETTaskCompletionSource<bool> tcs = new ETTaskCompletionSource<bool>();
			OnceWaitTimer timer = EntityFactory.CreateWithParent<OnceWaitTimer, ETTaskCompletionSource<bool>>(this, tcs);
			this.timers[timer.Id] = timer;
			AddToTimeId(tillTime, timer.Id);
			return await tcs.Task;
		}

		/// <summary>
		/// 创建一个RepeatedTimer
		/// </summary>
		/// <param name="time"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public long NewRepeatedTimer(long time, Action<bool> action)
		{
			if (time < 30)
			{
				throw new Exception($"repeated time < 30");
			}
			long tillTime = TimeHelper.Now() + time;
			RepeatedTimer timer = EntityFactory.CreateWithParent<RepeatedTimer, long, Action<bool>>(this, time, action);
			this.timers[timer.Id] = timer;
			AddToTimeId(tillTime, timer.Id);
			return timer.Id;
		}

		public RepeatedTimer GetRepeatedTimer(long id)
		{
			if (!this.timers.TryGetValue(id, out ITimer timer))
			{
				return null;
			}
			return timer as RepeatedTimer;
		}

		public void Remove(long id)
		{
			if (id == 0)
			{
				return;
			}
			ITimer timer;
			if (!this.timers.TryGetValue(id, out timer))
			{
				return;
			}
			this.timers.Remove(id);

			(timer as IDisposable)?.Dispose();
		}

		public long NewOnceTimer(long tillTime, Action action)
		{
			OnceTimer timer = EntityFactory.CreateWithParent<OnceTimer, Action>(this, action);
			this.timers[timer.Id] = timer;
			AddToTimeId(tillTime, timer.Id);
			return timer.Id;
		}

		public OnceTimer GetOnceTimer(long id)
		{
			if (!this.timers.TryGetValue(id, out ITimer timer))
			{
				return null;
			}
			return timer as OnceTimer;
		}

		public void AddToTimeId(long tillTime, long id)
		{
			this.TimeId.Add(tillTime, id);
			if (tillTime < this.minTime)
			{
				this.minTime = tillTime;
			}
		}
	}
}