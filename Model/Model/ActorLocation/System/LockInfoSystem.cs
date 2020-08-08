using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class LockInfoAwakeSystem : AwakeSystem<LockInfo, long, CoroutineLock>
	{
		public override void Awake(LockInfo self, long lockInstanceId, CoroutineLock coroutineLock)
		{
			self.CoroutineLock = coroutineLock;
			self.LockInstanceId = lockInstanceId;
		}
	}
}
