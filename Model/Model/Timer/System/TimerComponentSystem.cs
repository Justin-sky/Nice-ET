using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class TimerComponentAwakeSystem : AwakeSystem<TimerComponent>
	{
		public override void Awake(TimerComponent self)
		{
			TimerComponent.Instance = self;
		}
	}


	public class TimerComponentUpdateSystem : UpdateSystem<TimerComponent>
	{
		public override void Update(TimerComponent self)
		{
			self.Update();
		}
	}
}
