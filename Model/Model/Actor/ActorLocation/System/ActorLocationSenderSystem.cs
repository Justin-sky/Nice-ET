using System;

namespace NiceET
{
    public class ActorLocationSenderAwakeSystem : AwakeSystem<ActorLocationSender>
    {
        public override void Awake(ActorLocationSender self)
        {
            self.LastSendOrRecvTime = TimeHelper.Now();
            self.FailTimes = 0;
            self.ActorId = 0;

            StartAsync(self).Coroutine();
        }

        public async ETVoid StartAsync(ActorLocationSender self)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.ActorLocationSender, self.Id))
            {
                self.ActorId = await Game.Scene.GetComponent<LocationProxyComponent>().Get(self.Id);
            }
        }
    }

    public class ActorLocationSenderDestroySystem : DestroySystem<ActorLocationSender>
    {
        public override void Destroy(ActorLocationSender self)
        {
            Log.Debug($"actor location remove: {self.Id}");
            self.LastSendOrRecvTime = 0;
            self.ActorId = 0;
            self.FailTimes = 0;
        }
    }

    
}
