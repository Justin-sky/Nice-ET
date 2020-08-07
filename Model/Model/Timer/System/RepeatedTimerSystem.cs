using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class RepeatedTimerAwakeSystem : AwakeSystem<RepeatedTimer, long, Action<bool>>
	{
		public override void Awake(RepeatedTimer self, long repeatedTime, Action<bool> callback)
		{
			self.Awake(repeatedTime, callback);
		}
	}
}
