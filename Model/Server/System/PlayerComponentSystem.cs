using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class PlayerComponentSystem : AwakeSystem<PlayerComponent>
	{
		public override void Awake(PlayerComponent self)
		{
			self.Awake();
		}
	}
}
