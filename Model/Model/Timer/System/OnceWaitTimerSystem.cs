using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class OnceWaitTimerAwakeSystem : AwakeSystem<OnceWaitTimer, ETTaskCompletionSource<bool>>
	{
		public override void Awake(OnceWaitTimer self, ETTaskCompletionSource<bool> callback)
		{
			self.Callback = callback;
		}
	}
}
