using System;
using System.Collections.Generic;


namespace NiceET
{
	public class ConfigAwakeSystem : AwakeSystem<ConfigComponent>
	{
		public override void Awake(ConfigComponent self)
		{
			ConfigComponent.Instance = self;
			self.Awake();
		}
	}

	public class ConfigLoadSystem : LoadSystem<ConfigComponent>
	{
		public override void Load(ConfigComponent self)
		{
			self.Load();
		}
	}

	public class ConfigDestroySystem : DestroySystem<ConfigComponent>
	{
		public override void Destroy(ConfigComponent self)
		{
			ConfigComponent.Instance = null;
		}
	}

	
}