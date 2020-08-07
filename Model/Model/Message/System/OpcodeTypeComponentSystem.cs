using System;
using System.Collections.Generic;
using System.Text;

namespace NiceET
{
	public class OpcodeTypeComponentAwakeSystem : AwakeSystem<OpcodeTypeComponent>
	{
		public override void Awake(OpcodeTypeComponent self)
		{
			OpcodeTypeComponent.Instance = self;
			self.Load();
		}
	}


	public class OpcodeTypeComponentLoadSystem : LoadSystem<OpcodeTypeComponent>
	{
		public override void Load(OpcodeTypeComponent self)
		{
			self.Load();
		}
	}


	public class OpcodeTypeComponentDestroySystem : DestroySystem<OpcodeTypeComponent>
	{
		public override void Destroy(OpcodeTypeComponent self)
		{
			OpcodeTypeComponent.Instance = null;
		}
	}

}
