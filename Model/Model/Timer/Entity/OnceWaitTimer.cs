using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class OnceWaitTimer : Entity, ITimer
	{
		public ETTaskCompletionSource<bool> Callback { get; set; }

		public void Run(bool isTimeout)
		{
			ETTaskCompletionSource<bool> tcs = this.Callback;
			this.GetParent<TimerComponent>().Remove(this.Id);
			tcs.SetResult(isTimeout);
		}
	}
}
