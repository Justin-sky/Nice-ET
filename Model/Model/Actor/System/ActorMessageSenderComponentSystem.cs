using System;

namespace NiceET
{
    public class ActorMessageSenderComponentAwakeSystem : AwakeSystem<ActorMessageSenderComponent>
    {
        public override void Awake(ActorMessageSenderComponent self)
        {
            ActorMessageSenderComponent.Instance = self;

            self.TimeoutCheckTimer = TimerComponent.Instance.NewRepeatedTimer(10 * 1000, self.Check);
        }
    }

    public class ActorMessageSenderComponentDestroySystem : DestroySystem<ActorMessageSenderComponent>
    {
        public override void Destroy(ActorMessageSenderComponent self)
        {
            ActorMessageSenderComponent.Instance = null;
            TimerComponent.Instance.Remove(self.TimeoutCheckTimer);
            self.TimeoutCheckTimer = 0;
            self.TimeoutActorMessageSenders.Clear();
        }
    }

    
}
