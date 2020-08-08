using System;
using System.Collections.Generic;

namespace NiceET
{
    public class ActorMessageDispatcherComponentAwakeSystem : AwakeSystem<ActorMessageDispatcherComponent>
	{
		public override void Awake(ActorMessageDispatcherComponent self)
		{
			ActorMessageDispatcherComponent.Instance = self;
			self.Awake();
		}
	}

	public class ActorMessageDispatcherComponentLoadSystem : LoadSystem<ActorMessageDispatcherComponent>
	{
		public override void Load(ActorMessageDispatcherComponent self)
		{
			self.Load();
		}
	}

	public class ActorMessageDispatcherComponentDestroySystem : DestroySystem<ActorMessageDispatcherComponent>
	{
		public override void Destroy(ActorMessageDispatcherComponent self)
		{
			self.ActorMessageHandlers.Clear();
			ActorMessageDispatcherComponent.Instance = null;
		}
	}

	
}
