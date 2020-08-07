using System;
using System.Collections.Generic;

namespace NiceET
{
    public class MessageDispatcherComponentAwakeSystem : AwakeSystem<MessageDispatcherComponent>
	{
		public override void Awake(MessageDispatcherComponent self)
		{
			MessageDispatcherComponent.Instace = self;
			self.Load();
		}
	}

	public class MessageDispatcherComponentLoadSystem : LoadSystem<MessageDispatcherComponent>
	{
		public override void Load(MessageDispatcherComponent self)
		{
			self.Load();
		}
	}

	public class MessageDispatcherComponentDestroySystem : DestroySystem<MessageDispatcherComponent>
	{
		public override void Destroy(MessageDispatcherComponent self)
		{
			MessageDispatcherComponent.Instace = null;
			self.Handlers.Clear();
		}
	}

	
}
