using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class OnceTimerAwakeSystem : AwakeSystem<OnceTimer, Action<bool>>
	{
		public override void Awake(OnceTimer self, Action<bool> callback)
		{
			self.Callback = callback;
		}
	}
}
