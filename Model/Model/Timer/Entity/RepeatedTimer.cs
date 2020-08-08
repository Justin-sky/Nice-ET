using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class RepeatedTimer : Entity, ITimer
	{
		public void Awake(long repeatedTime, Action<bool> callback)
		{
			this.StartTime = TimeHelper.Now();
			this.RepeatedTime = repeatedTime;
			this.Callback = callback;
			this.Count = 1;
		}

		private long StartTime { get; set; }

		private long RepeatedTime { get; set; }

		// 下次一是第几次触发
		private int Count { get; set; }

		public Action<bool> Callback { private get; set; }

		public void Run(bool isTimeout)
		{
			++this.Count;
			TimerComponent timerComponent = this.GetParent<TimerComponent>();
			long tillTime = this.StartTime + this.RepeatedTime * this.Count;
			timerComponent.AddToTimeId(tillTime, this.Id);

			try
			{
				this.Callback?.Invoke(isTimeout);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			long id = this.Id;

			if (id == 0)
			{
				Log.Error($"RepeatedTimer可能多次释放了");
				return;
			}

			base.Dispose();

			this.StartTime = 0;
			this.RepeatedTime = 0;
			this.Callback = null;
			this.Count = 0;
		}
	}
}
