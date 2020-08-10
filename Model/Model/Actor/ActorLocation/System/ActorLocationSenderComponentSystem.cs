using System;

namespace NiceET
{
    public class ActorLocationSenderComponentAwakeSystem : AwakeSystem<ActorLocationSenderComponent>
    {
        public override void Awake(ActorLocationSenderComponent self)
        {
            ActorLocationSenderComponent.Instance = self;

            // 每10s扫描一次过期的actorproxy进行回收,过期时间是1分钟
            // 可能由于bug或者进程挂掉，导致ActorLocationSender发送的消息没有确认，结果无法自动删除，每一分钟清理一次这种ActorLocationSender
            self.CheckTimer = TimerComponent.Instance.NewRepeatedTimer(10 * 1000, self.Check);
        }
    }

    public class ActorLocationSenderComponentDestroySystem : DestroySystem<ActorLocationSenderComponent>
    {
        public override void Destroy(ActorLocationSenderComponent self)
        {
            ActorLocationSenderComponent.Instance = null;
            TimerComponent.Instance.Remove(self.CheckTimer);
            self.CheckTimer = 0;
        }
    }

   
}
