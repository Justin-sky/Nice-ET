using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class NumericComponentSystem : AwakeSystem<NumericComponent>
	{
		public override void Awake(NumericComponent self)
		{
			self.Awake();
		}
	}
}
